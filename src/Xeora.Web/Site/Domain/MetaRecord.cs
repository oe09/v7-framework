﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xeora.Web.Basics;

namespace Xeora.Web.Site
{
    public class MetaRecord : IMetaRecordCollection
    {
        private ConcurrentDictionary<Basics.MetaRecord.Tags, string> _Records;
        private ConcurrentDictionary<string, string> _CustomRecords;

        public MetaRecord()
        {
            this._Records = new ConcurrentDictionary<Basics.MetaRecord.Tags, string>();
            this._CustomRecords = new ConcurrentDictionary<string, string>();
        }

        public void Add(Basics.MetaRecord.TagSpaces tagSpace, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new NullReferenceException("Name can not be null!");

            if (string.IsNullOrEmpty(value))
                value = string.Empty;

            switch (tagSpace)
            {
                case Basics.MetaRecord.TagSpaces.name:
                    name = string.Format("name::{0}", name);

                    break;
                case Basics.MetaRecord.TagSpaces.httpequiv:
                    name = string.Format("httpequiv::{0}", name);

                    break;
                case Basics.MetaRecord.TagSpaces.property:
                    name = string.Format("property::{0}", name);

                    break;
            }

            this._CustomRecords.AddOrUpdate(name, value, (cName, cValue) => value);
        }

        public void Add(Basics.MetaRecord.Tags tag, string value)
        {
            if (string.IsNullOrEmpty(value))
                value = string.Empty;

            this._Records.AddOrUpdate(tag, value, (cName, cValue) => value);
        }

        public void Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new NullReferenceException("Name can not be null!");

            string dummy;
            this._CustomRecords.TryRemove(name, out dummy);
        }

        public void Remove(Basics.MetaRecord.Tags tag)
        {
            string dummy;
            this._Records.TryRemove(tag, out dummy);
        }

        public KeyValuePair<Basics.MetaRecord.Tags, string>[] CommonTags
        {
            get
            {
                KeyValuePair<Basics.MetaRecord.Tags, string>[] metaTags =
                    new KeyValuePair<Basics.MetaRecord.Tags, string>[this._Records.Keys.Count];

                int keyCount = 0;
                foreach (Basics.MetaRecord.Tags key in this._Records.Keys)
                {
                    string value;
                    this._Records.TryGetValue(key, out value);

                    metaTags[keyCount] = new KeyValuePair<Basics.MetaRecord.Tags, string>(key, value);
                    keyCount++;
                }

                return metaTags;
            }
        }

        public KeyValuePair<string, string>[] CustomTags
        {
            get
            {
                KeyValuePair<string, string>[] metaTags =
                    new KeyValuePair<string, string>[this._CustomRecords.Keys.Count];

                int keyCount = 0;
                foreach (string key in this._CustomRecords.Keys)
                {
                    string value;
                    this._CustomRecords.TryGetValue(key, out value);

                    metaTags[keyCount] = new KeyValuePair<string, string>(key, value);
                    keyCount++;
                }

                return metaTags;
            }
        }
    }
}
