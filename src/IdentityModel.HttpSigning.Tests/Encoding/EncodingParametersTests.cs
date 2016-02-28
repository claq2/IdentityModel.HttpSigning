﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using IdentityModel.HttpSigning;
using System.Net.Http;

namespace IdentityModel.HttpSigning.Tests
{
    public class EncodingParametersTests
    {
        EncodingParameters _subject = new EncodingParameters("abc");

        [Fact]
        public void ctor_should_require_accesstoken()
        {
            Assert.Throws<ArgumentNullException>(() => new EncodingParameters(null));
            Assert.Throws<ArgumentNullException>(() => new EncodingParameters(""));
        }

        [Fact]
        public void encoding_should_contain_basic_values()
        {
            var now = new DateTimeOffset(new DateTime(2016, 2, 3, 4, 5, 6));
            _subject.TimeStamp = now;

            var values = _subject.ToEncodedDictionary();

            values.Keys.Count.Should().Be(2);
            values.ContainsKey("at").Should().BeTrue();
            values["at"].Should().Be("abc");
            values.ContainsKey("ts").Should().BeTrue();
            values["ts"].Should().Be(now.ToEpochTime());
        }

        [Fact]
        public void encoding_should_emit_http_method()
        {
            _subject.HttpMethod = HttpMethod.Post;

            var values = _subject.ToEncodedDictionary();

            values.ContainsKey("m").Should().BeTrue();
            values["m"].Should().Be("POST");
        }

        [Fact]
        public void encoding_should_emit_host()
        {
            _subject.Host = "localhost:12345";

            var values = _subject.ToEncodedDictionary();

            values.ContainsKey("u").Should().BeTrue();
            values["u"].Should().Be("localhost:12345");
        }

        [Fact]
        public void encoding_should_emit_url_path()
        {
            _subject.UrlPath = "/foo";

            var values = _subject.ToEncodedDictionary();

            values.ContainsKey("p").Should().BeTrue();
            values["p"].Should().Be("/foo");
        }

        [Fact]
        public void encoding_should_emit_query_params()
        {
            _subject.QueryParameters.Add(new KeyValuePair<string, string>("foo", "bar1"));
            _subject.QueryParameters.Add(new KeyValuePair<string, string>("foo", "bar2"));

            var values = _subject.ToEncodedDictionary();

            values.ContainsKey("q").Should().BeTrue();
            values["q"].Should().BeAssignableTo<object[]>();
            var parts = (object[])values["q"];
            var keys = (IEnumerable<string>)parts[0];
            keys.Count().Should().Be(2);
            keys.Should().ContainInOrder(new string[] { "foo", "foo" });
            var value = (string)parts[1];
            value.Should().Be("EgePUeakH8URmSH3yz8zE39c0r4kYVYsuQRSZa_MdvQ");
        }

        [Fact]
        public void encoding_should_emit_header_list()
        {
            _subject.RequestHeaders.Add(new KeyValuePair<string, string>("foo", "bar1"));
            _subject.RequestHeaders.Add(new KeyValuePair<string, string>("foo", "bar2"));

            var values = _subject.ToEncodedDictionary();

            values.ContainsKey("h").Should().BeTrue();
            values["h"].Should().BeAssignableTo<object[]>();
            var parts = (object[])values["h"];
            var keys = (IEnumerable<string>)parts[0];
            keys.Count().Should().Be(2);
            keys.Should().ContainInOrder(new string[] { "foo", "foo" });
            var value = (string)parts[1];
            value.Should().Be("trcKLrzChz2G8_T50KpExyZ9DTfcsMTXsGGd3YwftLc");
        }

        [Fact]
        public void encoding_should_emit_body()
        {
            _subject.Body = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            var values = _subject.ToEncodedDictionary();

            values.ContainsKey("b").Should().BeTrue();
            var body = (string)values["b"];
            body.Should().Be("vnPfBFvKBXMv9S6m1FMSeSi1VLnnmqYXGr4xk9ImCp8");
        }
    }
}