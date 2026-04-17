using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Android.Util;
using Firebase.Firestore;
using Java.Util;
using OrganikaVer1.BusinessLogic;

namespace OrganikaVer1.Service
{
    public class EventsRepository
    {
        public static IListenerRegistration Registration;
        public static FirestoreEventListener FirestoreEventListener;

        // DELETE: just removes the event document from Firestore
        public static async Task DeleteAsync(object eventToDelete)
        {
            Model.Event tmpEvent = eventToDelete as Model.Event;
            try
            {
                await FirebaseFirestore.Instance
                    .Collection("events")
                    .Document(tmpEvent.Id)
                    .Delete();

                Log.Debug(ProManager.TAG, $"Delete event {tmpEvent.Id} success");
            }
            catch (Exception ex)
            {
                Log.Debug(ProManager.TAG, $"Delete event failed: " + ex.Message);
                throw new Exception("Delete event failed!");
            }
        }

        // GET BY ID: fetches a single event by its document ID
        public static async Task<object> GetByIdAsync(string eventId)
        {
            Model.Event newEvent = null;
            try
            {
                DocumentReference eventRef = FirebaseFirestore.Instance
                    .Collection("events")
                    .Document(eventId);

                var eventObject = await eventRef.Get();
                var doc = (DocumentSnapshot)eventObject;

                newEvent = new Model.Event()
                {
                    Id = eventId,
                    EventName = doc.Get("EventName").ToString(),
                    AssignedTime = doc.Contains("AssignedTime") ? int.Parse(doc.Get("AssignedTime").ToString()) : 0,
                    ScheduledHours = doc.Contains("ScheduledHours") ? int.Parse(doc.Get("ScheduledHours").ToString()) : 0,
                    Completed = doc.Contains("Completed") ? int.Parse(doc.Get("Completed").ToString()) : 0,
                    UserId = doc.Contains("UserId") ? doc.Get("UserId").ToString() : ""
                };

                Log.Debug(ProManager.TAG, $"GetEventById: success");
                return newEvent;
            }
            catch (FirebaseFirestoreException ex)
            {
                Log.Debug(ProManager.TAG, $"GetEventById failed: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Debug(ProManager.TAG, $"GetEventById general error: {ex.Message}");
                return null;
            }
        }

        // INSERT: saves all event fields to the "events" collection
        public static async Task InsertAsync(object item)
        {
            Model.Event tmpEvent = item as Model.Event;
            try
            {
                HashMap eventMap = new HashMap();
                eventMap.Put("EventName", tmpEvent.EventName);
                eventMap.Put("AssignedTime", tmpEvent.AssignedTime);
                eventMap.Put("ScheduledHours", tmpEvent.ScheduledHours);
                eventMap.Put("Completed", tmpEvent.Completed);
                eventMap.Put("UserId", ProManager.CurrentUser?.Id ?? "");

                DocumentReference eventReference = FirebaseFirestore.Instance
                    .Collection("events")
                    .Document();

                await eventReference.Set(eventMap);
                Log.Debug(ProManager.TAG, $"Add Event to Firestore completed");
            }
            catch (FirebaseFirestoreException ex)
            {
                Log.Error(ProManager.TAG, $"Add Event to Firestore failed: {ex.Message}");
                throw new Exception("Add Event to Firestore failed");
            }
            catch (Exception ex)
            {
                Log.Error(ProManager.TAG, $"Add Event to Firestore failed: {ex.Message}");
                throw new Exception("Add Event to Firestore failed");
            }
        }

        // UPDATE: updates event fields in the "events" collection
        public static async Task UpdateAsync(object item)
        {
            Model.Event tmpEvent = item as Model.Event;
            try
            {
                DocumentReference eventRef = FirebaseFirestore.Instance
                    .Collection("events")
                    .Document(tmpEvent.Id);

                await eventRef.Update("EventName", tmpEvent.EventName);
                await eventRef.Update("AssignedTime", tmpEvent.AssignedTime);
                await eventRef.Update("ScheduledHours", tmpEvent.ScheduledHours);
                await eventRef.Update("Completed", tmpEvent.Completed);

                Log.Debug(ProManager.TAG, $"Update event {tmpEvent.EventName} success");
            }
            catch (Exception ex)
            {
                Log.Debug(ProManager.TAG, $"Update event {tmpEvent.EventName} failed: " + ex.Message);
                throw new Exception($"Update event {tmpEvent.EventName} failed");
            }
        }

        // GET ALL: fetches all events from the "events" collection
        public static async Task<List<object>> GetAllAsync()
        {
            List<object> events = new List<object>();
            try
            {
                var documents = await FirebaseFirestore.Instance.Collection("events").Get();
                var firestoreEventsCollection = (QuerySnapshot)documents;

                if (!firestoreEventsCollection.IsEmpty)
                {
                    foreach (DocumentSnapshot item in firestoreEventsCollection.Documents)
                    {
                        Model.Event ev = new Model.Event()
                        {
                            Id = item.Id,
                            EventName = item.Get("EventName").ToString(),
                            AssignedTime = item.Contains("AssignedTime") ? int.Parse(item.Get("AssignedTime").ToString()) : 0,
                            ScheduledHours = item.Contains("ScheduledHours") ? int.Parse(item.Get("ScheduledHours").ToString()) : 0,
                            Completed = item.Contains("Completed") ? int.Parse(item.Get("Completed").ToString()) : 0,
                            UserId = item.Contains("UserId") ? item.Get("UserId").ToString() : ""
                        };
                        events.Add(ev);
                    }
                    Log.Debug(ProManager.TAG, $"GetEventsCollection: loaded {events.Count} events");
                }
                return events;
            }
            catch (FirebaseFirestoreException ex)
            {
                Log.Debug(ProManager.TAG, $"GetEventsCollection failed: {ex.Message}");
                return events;
            }
            catch (Exception ex)
            {
                Log.Debug(ProManager.TAG, $"GetEventsCollection general error: {ex.Message}");
                return events;
            }
        }

        // LISTENER: listens to real-time changes on the "events" collection
        public static void FetchEventsListener()
        {
            FirestoreEventListener = new FirestoreEventListener();
            Registration = FirebaseFirestore.Instance
                .Collection("events")
                .AddSnapshotListener(FirestoreEventListener);
        }

        public static void StopEventsListener()
        {
            Registration?.Remove();
            Registration = null;
            FirestoreEventListener = null;
        }
    }
}