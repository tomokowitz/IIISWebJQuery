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
/*jslint onevar: true, undef: true, newcap: true, regexp: true, 
plusplus: true, bitwise: true, devel: true, maxerr: 50 */
/*global window: true, jQuery:true, $:true, document:true, mstats:true */

//(function(iiisweb, $) {

//    $.widget('iiisweb.layoutManager', {
//        options: {
//            layout: 'dashboard',
//            subscribe: function () { mstats.log('The subscribe option on layoutManager has not been set'); },
//            pinnedSite: null,
//            charts: null,
//            header: null,
//            infoPane: null,
//            summaryPane: null,
//            vehicleList: null
//        },

//        _create: function() {
//            var state = $.bbq.getState() || {  };

//            // add on helper methods for invoking public methods on widgets
//            iiisweb.setupWidgetInvocationMethods(this, this.options, ['charts', 'header', 'infoPane', 'summaryPane', 'vehicleList']);

//            this._subscribeToGlobalEvents();
//            this._subscribeToHashChange();

//            if (state && state.layout) {
//                this._setOption('layout', state.layout);
//            }

//            this._changeLayout(state);
//        },

//        _subscribeToGlobalEvents: function() {
//            var state = {  },
//                that = this;

//            this.options.subscribe(iiisweb.events.vehicle.deleted,
//                function() {
//                    // we need to refresh the vehicle list, summary reminders, 
//                    // fleet statistics, and redirect to the dashboard
//                    that._summaryPane('requeryStatistics');
//                    that._summaryPane('requeryReminders');
//                    that._vehicleList('requeryData');
//                    that._charts('requeryData');
//                    state.layout = 'dashboard';
//                    $.bbq.pushState(state, 2);
//                }, this);

//            this.options.subscribe(iiisweb.events.vehicle.reminders.fulfilled,
//                function() {
//                    // we need to refresh the summary reminders, fleet statistics, 
//                    // details pane, reminders pane and jump list
//                    that._summaryPane('requeryStatistics');
//                    that._summaryPane('requeryReminders');
//                    that._infoPane('requeryVehicleDetails');
//                    that._infoPane('requeryReminders');
//                    that._charts('requeryData');

//                    that.options.pinnedSite.requeryJumpList();
//                }, this);
//        },

//        _changeLayout: function(state) {
//            this._setOption('layout', state.layout || 'dashboard');

//            this._setupInfoPaneOptions(state);
//            switch (this.options.layout) {
//                case 'dashboard':
//                    this._header('option', 'title', 'Dashboard');
//                    this._goToDashboardLayout();
//                    break;
//                case 'charts':
//                    this._goToChartsLayout();
//                    this._header('option', 'title', 'Charts');
//                    break;
//                case 'details':
//                    // The first time we transition to details, the selected vehicle data is not available,
//                    // in that case the vehicleDetail widgets takes over the responsibility of setting the title
//                    // because it contains additional information to display there
//                    this._setHeaderForSelectedVehicle('Details');
//                    this._goToDetailsLayout();
//                    break;
//                case 'fillups':
//                    this._setHeaderForSelectedVehicle('Fill ups');
//                    this._goToDetailsLayout();
//                    break;
//                case 'reminders':
//                    this._setHeaderForSelectedVehicle('Reminders');
//                    this._goToDetailsLayout();
//                    break;
//            }
//        },

//        _setHeaderForSelectedVehicle: function(titleBase) {
//            var vehicleName = this._infoPane('getSelectedVehicleName'),
//                title = titleBase;

//            if (vehicleName) {
//                title = title + " for " + vehicleName;
//            }

//            this._header('option', 'title', title);
//        },

//        _subscribeToHashChange: function() {
//            var that = this;
//            $(window).bind('hashchange.layoutManager', function(event) {
//                var state = $.deparam.fragment(true);
//                that._changeLayout(state);
//            });
//        },

//        _setupInfoPaneOptions: function(state) {
//            this._vehicleList('option', 'selectedVehicleId', state.vid);
//            this._infoPane('option', 'selectedVehicleId', state.vid);
//            this._infoPane('option', 'activePane', state.layout);
//        },

//        _goToChartsLayout: function() {
//            this._summaryPane('moveOffScreen');
//            this._vehicleList('moveOffScreen');
//            this._infoPane('moveOffScreenToRight');
//            this._charts('moveOnScreenFromRight');
//        },

//        _goToDetailsLayout: function() {
//            this._charts('moveOffScreenToRight');
//            this._summaryPane('moveOffScreen');
//            this._infoPane('moveOnScreenFromRight');
//            this._vehicleList('moveOnScreen');
//            this._vehicleList('goToDetailsLayout');
//        },

//        _goToDashboardLayout: function() {
//            this._charts('moveOffScreenToRight');
//            this._summaryPane('moveOnScreen');
//            this._vehicleList('moveOnScreen');
//            this._infoPane('moveOffScreenToRight');
//            this._vehicleList('goToDashboardLayout');
//        },

//        destroy: function() {
//            // will unbind event handlers namespaced with the widget's name
//            $.Widget.prototype.destroy.call(this);
//        }
//    });

$(function () {
    $(document).ajaxError(function (ev, jqXHR, settings, errorThrown) {
        		if (jqXHR.status === 200) {
        			if (jqXHR.responseText.indexOf('Mileage Stats Sign In') !== -1) {
        				window.location.replace(iiisweb.getRelativeEndpointUrl('/Auth/SignIn'));
        			} else if (jqXHR.responseText.indexOf('Mileage Stats | Accident!') !== -1) {
        				window.location.replace(iiisweb.getRelativeEndpointUrl('/GenericError.htm'));
        			}
        		}
    });
    //iiisweb.setupWidgetInvocationMethods(this, this.options, ['grid']);
    //_subscribeToGlobalEvents: function() {
    //    var state = {  },
    //        that = this;

    //    this.options.subscribe(mstats.events.vehicle.deleted,
    //        function() {
    //            // we need to refresh the vehicle list, summary reminders, 
    //            // fleet statistics, and redirect to the dashboard
    //            that._summaryPane('requeryStatistics');
    //            that._summaryPane('requeryReminders');
    //            that._vehicleList('requeryData');
    //            that._charts('requeryData');
    //            state.layout = 'dashboard';
    //            $.bbq.pushState(state, 2);
    //        }, this);

    //    this.options.subscribe(mstats.events.vehicle.reminders.fulfilled,
    //        function() {
    //            // we need to refresh the summary reminders, fleet statistics, 
    //            // details pane, reminders pane and jump list
    //            that._summaryPane('requeryStatistics');
    //            that._summaryPane('requeryReminders');
    //            that._infoPane('requeryVehicleDetails');
    //            that._infoPane('requeryReminders');
    //            that._charts('requeryData');

    //            that.options.pinnedSite.requeryJumpList();
    //        }, this);
    //},
    
    //var elem = $(this).attr('grid');
    var elem = $(this).find('grid');
    var url = elem.data('source');
    var grid;
    var onSuccessFunc = function (response) {  // grid is starting to render since adding this TVO
        alert("The result contains " + response.JsonTotal + " records."); // error in response.records ... response only has mapping fields as properties
        grid.render(response);
    };
    grid = $('#grid').grid({            //arguments seem to be passed in as jsConfiguration, so a dataManager object and/or sendRequest argument could be added in as well?
        dataKey: "INSP_ID",
        uiLibrary: "jqueryui", // could changing this have made widgets usable? TVO
        columns: [
            { field: "Milepost", width: 100, sortable: true },
            { field: "INSP_ID", width: 100, sortable: true },
            { field: "UC_Read_Date", width: 100, title: "Reading Date", sortable: true },
            { field: "Inspect_Date", width: 100, title: "Inspection Date", sortable: true }
            //,
            //{ title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            //{ title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": Remove }  }
        ],
        //dataSource: iiisweb.dataManager.sendRequest,
        //dataSource: { url: iiisweb.data.url, sendRequest: iiisweb.data.sendRequest, success: onSuccessFunc },
        dataSource: { url: '/Reading/ReadingList', data: {}, success: onSuccessFunc}, // Need to try this with dataType somewhere else TVO
        //dataType: "json",
        //invalidateData: iiisweb.dataManager.resetData,
        // publish: iiisweb.pubsub.publish,
        pager: {
            enable: true, limit: 5, sizes: [2, 5, 10, 20]
        }
    });
    });
	//readingGrid = $('#grid').readingGrid({          // to use this, must convert grid into widget
	//    sendRequest: iiisweb.dataManager.sendRequest,
	//    invalidateData: iiisweb.dataManager.resetData,
	//    publish: iiisweb.pubsub.publish
        
	//})
	//charts = $('#main-chart').charts({
	//	sendRequest: mstats.dataManager.sendRequest,
	//	invalidateData: mstats.dataManager.resetData
	//});

	//$('body').layoutManager({
	//	subscribe: iiisweb.pubsub.subscribe,
	//	pinnedSite: iiisweb.pinnedSite,
	//	summaryPane: summaryPane,
	//	readingList: readingList,
	//	readingGrid: readingGrid
	//});
//});
