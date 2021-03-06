﻿//===================================================================================
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

$(function () {
	var readingGrid,
        readingFilter,
        readingForm,
        readingList;

	// setup default error handler for redirects due to session timeout.
	$(document).ajaxError(function (ev, jqXHR, settings, errorThrown) {
		if (jqXHR.status === 200) {
			if (jqXHR.responseText.indexOf('Mileage Stats Sign In') !== -1) {
				window.location.replace(mstats.getRelativeEndpointUrl('/Auth/SignIn'));
			} else if (jqXHR.responseText.indexOf('Mileage Stats | Accident!') !== -1) {
				window.location.replace(mstats.getRelativeEndpointUrl('/GenericError.htm'));
			}
		}
	});

	//$('#notification').status({
	//	subscribe: iiisweb.pubsub.subscribe
	//});

	//header = $('#header').header();

	//iiisweb.pinnedSite.intializeData(iiisweb.dataManager.sendRequest);

	//if (!window.location.pathname.match(/Dashboard/)) {
	//	return; // only enable widgets on the dashboard
	//}

	//readingList = $('#readings').readingList({
	//	// This allows the vehicleList to participate
	//	// in global messaging with other widgets
	//	publish: iiisweb.pubsub.publish,

	//	// This overrides the vehicleLists default ($.ajax) 
	//	// way of getting data so we can inject a caching layer
	//	sendRequest: iiisweb.dataManager.sendRequest,

	//	// this allows the vehicleList to invalidate data
	//	// stored in the data cache.  
	//	invalidateData: iiisweb.dataManager.resetData,

	//	// the ID of the element containing the vehicle list template
	//	templateId: '#iiisweb-reading-list-template'
	//});

	//infoPane = $('#info').infoPane({
	//	sendRequest: mstats.dataManager.sendRequest,
	//	invalidateData: mstats.dataManager.resetData,
	//	publish: mstats.pubsub.publish,
	//	header: header
	//});

	//summaryPane = $('#summary').summaryPane({
	//	sendRequest: mstats.dataManager.sendRequest,
	//	invalidateData: mstats.dataManager.resetData,
	//	publish: mstats.pubsub.publish,
	//	header: header
	//});
	Grid = $('#grid').grid({          // to use this, must convert grid into widget
	    sendRequest: iiisweb.dataManager.sendRequest,
	    invalidateData: iiisweb.dataManager.resetData,
	    publish: iiisweb.pubsub.publish
        
	})
	//charts = $('#main-chart').charts({
	//	sendRequest: mstats.dataManager.sendRequest,
	//	invalidateData: mstats.dataManager.resetData
	//});
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

	$('body').uc_layout({
		subscribe: iiisweb.pubsub.subscribe,
		//pinnedSite: iiisweb.pinnedSite,
		//summaryPane: summaryPane,
		//readingList: readingList,
		Grid: grid
	});
});
