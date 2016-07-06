
/*jslint onevar: true, undef: true, newcap: true, regexp: true, 
plusplus: true, bitwise: true, devel: true, maxerr: 50 */
/*global window: true, jQuery:true, $:true, document:true, mstats:true */

(function (iiisweb, $) {
    $.widget('iiisweb.underclearance', {
        // default options
        options: {
            // Default to $.ajax when sendRequest is undefined.
            // The extra function indirection allows $.ajax substitution because 
            // the widget framework is using the real $.ajax during options initialization.
            sendRequest: function (ajaxOptions) { $.ajax(ajaxOptions); },

            invalidateData: function () {
                iiisweb.log('The invalidateData option on readingGrid has not been set');
            }
            //,

            //header: null
        },
        readingGrid: null,

        _create: function () {
            this._setupGridWidget();

            // add on helper methods for invoking public methods on widgets
            iiisweb.setupWidgetInvocationMethods(this, this.options, ['Grid']);
        },

        //// initialize the statistics widget
        _setupGridWidget: function () {
            var elem = this.element.find('#readings-grid-table');

            grid = $("#grid").grid({
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


