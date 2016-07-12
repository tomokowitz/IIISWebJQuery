
/*jslint onevar: true, undef: true, newcap: true, regexp: true, 
plusplus: true, bitwise: true, devel: true, maxerr: 50 */
/*global window: true, jQuery:true, $:true, document:true, mstats:true */

(function (iiisweb, $) {
    $.widget('iiisweb.uc_grid', {
        // default options
        options: {
            // Default to $.ajax when sendRequest is undefined.
            // The extra function indirection allows $.ajax substitution because 
            // the widget framework is using the real $.ajax during options initialization.

            publish: function () {
                mstats.log('The publish option on summaryPane has not been set');
            },
            sendRequest: function (ajaxOptions) { $.ajax(ajaxOptions); },

            invalidateData: function () {
                iiisweb.log('The invalidateData option on readingGrid has not been set');
            }
            //,

            //header: null
        },
        Grid: null,

        _create: function () {
            this._setupGrid();

            // add on helper methods for invoking public methods on widgets
            //iiisweb.setupWidgetInvocationMethods(this, this.options, ['Grid']);
        },

        //// initialize the statistics widget
        _setupGrid: function () {
            var elem = $(this).find('grid');
            //var strUrl = elem.data('source');
            var strUrl = $(this).data('source');
            var strUrlGrid = $(elem).data('source');

            var grid;
            var onSuccessFunc = function (response) {  // grid is starting to render since adding this TVO
                alert("The result contains " + response.JsonTotal + " records."); // error in response.records ... response only has mapping fields as properties
                grid.render(response);
            };

            
            //grid = $("#grid").grid({//arguments seem to be passed in as jsConfiguration, so a dataManager object and/or sendRequest argument could be added in as well?
            this.Grid = elem.grid({
                dataKey: "INSP_ID",
                uiLibrary: "bootstrap",
                columns: [
                    { field: "Milepost", width: 100, sortable: true },
                    { field: "INSP_ID", sortable: true },
                    { field: "UC_Read_Date", title: "Reading Date", sortable: true },
                    { field: "Inspect_Date", title: "Inspection Date", sortable: true }
                    //,
                    //{ title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
                    //{ title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": Remove }  }
                ],
                //dataSource: { url: strUrl, data: {}, success: onSuccessFunc, dataType: "json" },
                dataSource: { url: iiisweb.data.url, sendRequest: iiisweb.data.sendRequest, success: onSuccessFunc },
                pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
            });

            /* this.readingGrid = elem.readingGrid({
                sendRequest: this.options.sendRequest,
                dataUrl: elem.data('url'),
                invalidateData: this.options.invalidateData
                //templateId: '#fleet-statistics-template'
            }); */
        },


        // cleanup 
        destroy: function () {
            $.Widget.prototype.destroy.call(this); // default destroy

            this._readingGrid('destroy');

        }

    });
}(this.iiisweb, jQuery));


