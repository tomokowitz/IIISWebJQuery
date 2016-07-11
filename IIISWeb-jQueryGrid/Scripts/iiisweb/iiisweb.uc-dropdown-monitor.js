//===================================================================================
// Microsoft patterns & practices
// Silk : Web Client Guidance
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
/*jslint onevar: true, undef: true, newcap: true, regexp: true, plusplus: true, bitwise: true, devel: true, maxerr: 50 */
/*global jQuery */
(function (iiisweb, $) {

    iiisweb.UCDropDownMonitor = function UCDropDownMonitor(publish, sendRequest) {

        if (!(this instanceof UCDropDownMonitor)) {
            return new UCDropDownMonitor(publish, sendRequest);
        }

        var that = {};

        that.initialize = function () {
            var $readingsForm = $('#container readings'),
                $divSelect = $('#Year', $readingsForm),
                $featSelect = $('#MakeName', $readingsForm),
                $rDateSelect = $('#ModelName', $readingsForm),

                makesUrl = $readingsForm.data('makes-url'),
                modelsUrl = $readingsForm.data('models-url');

            $readingsForm.find('input[name="UpdateMakes"]').remove()
            .end()
            .find('input[name="UpdateModels"]').remove();

            $yearSelect.change(function () {
                $makeSelect.children().not(':first').remove();
                $modelSelect.children().not(':first').remove();

                sendRequest({
                    url: makesUrl,
                    data: { year: $yearSelect.val() },
                    cache: false,
                    success: function (data) {
                        that._updateList(data, $makeSelect);
                    },
                    error: function () {
                        that._publishError('Could not load vehicle data lists.');
                    }
                });
            });

            $makeSelect.change(function () {
                $modelSelect.children().not(':first').remove();

                sendRequest({
                    url: modelsUrl,
                    cache: false,
                    data: { year: $yearSelect.val(), make: $makeSelect.val() },
                    success: function (data) {
                        that._updateList(data, $modelSelect);
                    },
                    error: function () {
                        that._publishError('Could not load vehicle data lists.');
                    }
                });
            });
        };

        that._updateList = function (data, $selectList) {
            $.each(data,
              function (key, value) {
                  $selectList.append(
                    $('<option></option>')
                    .attr('value', value)
                    .text(value)
                    );
              });
        };

        that._publishError = function (message) {
            publish(mstats.events.status,
               {
                   type: 'loadError',
                   message: message
               });
        };

        return that;
    };

} (this.iiisweb = this.iiisweb || {}, jQuery));