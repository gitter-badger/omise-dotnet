﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Omise.Models;
using System.IO;
using System.Text;
using System.Net.Http;

namespace Omise {
    public interface IListable<TModel> : IResource<TModel>
        where TModel: ModelBase {
    }

    public static class Listables {
        static readonly Serializer serializer = new Serializer();

        public class ListOptions {
            public int? Offset { get; internal set; }
            public int? Limit { get; internal set; }
            public DateTime? From { get; internal set; }
            public DateTime? To { get; internal set; }
            public Ordering? Ordering { get; internal set; }

            public bool IsEmpty() {
                return Offset == null &&
                Limit == null &&
                From == null &&
                To == null &&
                Ordering == null;
            }
        }

        public static async Task<ScopedList<TResult>> GetList<TResult>(
            this IListable<TResult> resource,
            int? offset = null,
            int? limit = null,
            DateTime? from = null,
            DateTime? to = null,
            Ordering? ordering = null
        ) where TResult: ModelBase {

            var opts = new ListOptions
            {
                Offset = offset,
                Limit = limit,
                From = from,
                To = to,
                Ordering = ordering
            };
                        
            var path = resource.BasePath;
            if (!opts.IsEmpty()) {
                var content = serializer.ExtractFormValues(opts);
                path += "?" + await content.ReadAsStringAsync();
            }

            // TODO: Actual list container type that can be re-requested and supports paging as well.
            return await resource.Requester.Request<ScopedList<TResult>>(
                resource.Endpoint,
                "GET",
                path
            );
        }
    }
}

