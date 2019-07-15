using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenTelemetry.Context;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Sampler;

namespace src
{
    public class AzureDiagnosticListener: IDisposable, IObserver<DiagnosticListener>, IObserver<KeyValuePair<string, object>>
    {
        private readonly ITracer _tracer;
        // ugh
        private readonly ConcurrentDictionary<Activity, IScope> _scopes = new ConcurrentDictionary<Activity, IScope>();

        public AzureDiagnosticListener(ITracer tracer)
        {
            _tracer = tracer;
        }

        private IDisposable _subscription;

        public void Subscribe()
        {
            if (_subscription == null)
            {
                _subscription = DiagnosticListener.AllListeners.Subscribe(this);
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {

            if (value.Key.EndsWith("Start"))
            {
                OnStartActivity(Activity.Current, value.Value);
            }
            else if (value.Key.EndsWith("Stop"))
            {
                OnStopActivity(Activity.Current, value.Value);
            }
            else if (value.Key.EndsWith("Exception"))
            {
                OnException(Activity.Current, value.Value);
            }
            else
            {
                OnCustom(value.Key, Activity.Current, value.Value);
            }
        }

        private void OnStartActivity(Activity current, object valueValue)
        {
            var span = _tracer.SpanBuilder(current.OperationName)
                .SetSampler(Samplers.AlwaysSample)
                .StartSpan();

            _scopes.TryAdd(current, _tracer.WithSpan(span));
        }

        private void OnStopActivity(Activity current, object valueValue)
        {
            var span = _tracer.CurrentSpan;
            foreach (var keyValuePair in current.Tags)
            {
                span.SetAttribute(keyValuePair.Key, keyValuePair.Value);
            }

            _scopes.TryRemove(current, out var scope);

            scope?.Dispose();
        }

        private void OnException(Activity current, object valueValue)
        {
            var span = _tracer.CurrentSpan;
            foreach (var keyValuePair in current.Tags)
            {
                span.SetAttribute(keyValuePair.Key, keyValuePair.Value);
            }

            span.Status= Status.Unknown;

            _scopes.TryRemove(current, out var scope);
            scope?.Dispose();
        }

        private void OnCustom(string valueKey, Activity current, object valueValue)
        {
        }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name.StartsWith("Azure"))
            {
                value.Subscribe(this);
            }
        }
    }
}
