using System;

namespace Events
{
    public static class AlertEventsContainer
    {
        public static Action OnDehydratedAlertDisplayed { get; set; }
        public static Action OnStarvingAlertDisplayed { get; set; }
    }
}