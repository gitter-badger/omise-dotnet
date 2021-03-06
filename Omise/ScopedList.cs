﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Omise {
    [JsonObject]
    public class ScopedList<T> : IEnumerable<T> {
        IList<T> data;

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("from")]
        public DateTime From { get; set; }

        [JsonProperty("to")]
        public DateTime To { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        // TODO: "total" JSON field?
        [JsonIgnore]
        public int Count { get { return data?.Count ?? 0; } }

        [JsonProperty("data")]
        public IList<T> Data {
            get { return data; }
            set { data = new ReadOnlyCollection<T>(value); }
        }

        public T this[int indexer] {
            get { return data[indexer]; }
            set { data[indexer] = value; }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (Data ?? Enumerable.Empty<T>()).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return (Data ?? Enumerable.Empty<T>()).GetEnumerator();
        }
    }
}

