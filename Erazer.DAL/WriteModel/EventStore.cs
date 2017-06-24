﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Framework.Events;
using EventStore.ClientAPI;

namespace Erazer.DAL.WriteModel
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreConnection _storeConnection;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public EventStore(IEventStoreConnection storeConnection, IEventPublisher eventPublisher, IMapper mapper)
        {
            _storeConnection = storeConnection;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task Save(Guid aggregateId, IEnumerable<IEvent> events)
        {
            var eventData = _mapper.Map<IEnumerable<EventData>>(events);
            await _eventPublisher.Publish(events);
            await _storeConnection.AppendToStreamAsync(aggregateId.ToString(), ExpectedVersion.Any, eventData);
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion)
        {
            var startPosition = StreamPosition.Start;
            if (fromVersion > -1)
                startPosition = fromVersion + StreamPosition.Start;

            // TODO FIX HARD LIMIT OF 200 
            var eventCollection = await _storeConnection.ReadStreamEventsForwardAsync(aggregateId.ToString(), startPosition, 200, false);
            return _mapper.Map<IEnumerable<IEvent>>(eventCollection.Events);
        }
    }
}