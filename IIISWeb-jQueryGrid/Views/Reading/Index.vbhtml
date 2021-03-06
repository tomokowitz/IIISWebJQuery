﻿@ModelType IIISWeb_jQueryGrid.Models.ReadingModel


<!DOCTYPE html>

<html>
<head>
    <meta name = "viewport" content="width=device-width" />
    <title>How To use jQuery Grid With ASP.NET MVC</title>
    @*<link href="~/Content/tether/tether-theme-arrows-dark.css" rel="stylesheet" type="text/css">
    <link href="~/Content/tether/tether-theme-arrows-dark.min.css" rel="stylesheet" type="text/css">
    <link href="~/Content/tether/tether-theme-arrows.css" rel="stylesheet" type="text/css">
    <link href="~/Content/tether/tether-theme-arrows.min.css" rel="stylesheet" type="text/css">
    <link href="~/Content/tether/tether-theme-basic.css" rel="stylesheet"type="text/css">
    <link href="~/Content/tether/tether-theme-basic.min.css" rel="stylesheet" type="text/css">
    <link href="~/Content/tether/tether.css" rel="stylesheet" type="text/css">
    <link href="~/Content/tether/tether.min.css" rel="stylesheet" type="text/css">*@
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="~/Content/bootstrap-theme.min.css" rel="stylesheet" type="text/css">
    <link href="http://code.jquery.com/ui/1.9.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css">
    <link href="~/Content/grid-0.4.3.min.css" rel="stylesheet" type="text/css">

    @*<script src="~/Scripts/tether/tether.min.js" type="text/javascript"></script>*@

    
    <script src="~/Scripts/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="~/Scripts/jquery-ui-1.11.4.min.js" type="text/javascript"></script>
    <script src="~/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.utils.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.events.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.pubsub.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.data.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.uc-dropdown-monitor.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.grid_tvo-0.0.1.js" type="text/javascript"></script>
    @*<script src="~/Scripts/iiisweb/iiisweb.uc_grid.js" type="text/javascript"></script>*@
    <script src="~/Scripts/iiisweb/iiisweb.underclearance.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.js" type="text/javascript"></script>
    @*<script src="~/Scripts/grid-0.4.3.js" type="text/javascript"></script>*@
   
    <script type="text/javascript">
        var grid;
        function Add() {
            $("#Milepost").val("");
            $("#INSP_ID").val("");
            $("#UC_Read_Date").val("");
            $("#Inspect_Date").val("");
            $("#readingModal").modal("show");
        }
        //function Edit(e) {
        //    $("#Milepost").val(e.data.Milepost);
        //    $("#INSP_ID").val(e.data.record.INSP_ID);
        //    $("#UC_Read_Date").val(e.data.record.UC_Read_Date);
        //    $("#Inspect_Date").val(e.data.record.Inspect_Date);
        //    $("#readingModal").modal("show");
        //}
        function Save() {
            var reading = {
                Milepost: $("#Milepost").val(),
                INSP_ID: $("#INSP_ID").val(),
                UC_Read_Date: $("#UC_Read_Date").val(),
                Inspect_Date: $("#Inspect_Date").val()
            };
            $.ajax({ url: "Home/Save", type: "POST", data: { reading: reading } })
                .done(function () {
                    grid.reload();
                    $("#readingModal").modal("hide");
                })
                .fail(function () {
                    alert("Unable to save.");
                    $("#readingModal").modal("hide");
                });
        }
        //function Remove(e) {
        //    $.ajax({ url: "Home/Remove", type: "POST", data: { id: e.data.INSP_ID } })
        //        .done(function () {
        //            grid.reload();
        //        })
        //        .fail(function () {
        //            alert("Unable to remove.");
        //        });
        //}
        function Search() {
            grid.reload({ searchString: $("#search").val() });
        }
        function DivPop() {

            var result;
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: "/Reading/GetDivisions" ,
                data: {},
                dataType: "json",
                success: function (response) {
                    //Your code here
                    //data should be your json result
                    //result = response;
                    $("#ddlDiv option").remove(); // Remove all <option> child tags.

                    $.each(response, function (index, item) {
                        $("#ddlDiv").append(
                            $("<option></option>")
                                .text(item)
                                .val(index)
                            );
                    });
                    alert("dropdown loaded successfully!!!");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
            });
            //$("#ddlDiv option").remove(); // Remove all <option> child tags.

            //$.each(data.divisions, function(index,item){
            //    $("#ddlDiv").append(
            //        $("<option></option>")
            //            .text()
            //            .val()
            //        );
            //});
        
        }
        //function DivSelect() {

        //    var id = $(this).find(':selected')[0].id;
        //    $.ajax({
        //        url: "/Reading/ReadingList",
        //        type: "get",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            //Your code here
        //            //data should be your json result
        //            grid.reload({ searchString: "DIVISION_DESC='" && id && "'" })
        //            alert("dropdown selection change successful!!!");
        //        },
        //        error: function (xhr, ajaxOptions, thrownError) {
        //            alert(xhr.status);
        //            alert(thrownError);
        //        }
        //    });
        //}
        $(document).ready(function () {
        //    //grid = $("#grid").grid({
        //    //    dataKey: "INSP_ID",
        //    //    uiLibrary: "bootstrap",
        //    //    columns: [
        //    //        { field: "Milepost", width: 100, sortable: true },
        //    //        { field: "INSP_ID", sortable: true },
        //    //        { field: "UC_Read_Date", title: "Reading Date", sortable: true },
        //    //        { field: "Inspect_Date", title: "Inspection Date", sortable: true }
        //    //        //,
        //    //        //{ title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
        //    //        //{ title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": Remove }  }
        //    //    ],
        //    //    pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
        //    //});
        //    // need to populate dropdowns here???
            DivPop();
           
            $("#btnAddReading").on("click", Add);
            $("#btnSave").on("click", Save);
            $("#btnSearch").on("click", Search);
        //    // add dropdown and other grid events registration here???
        //    //$("#gridDDLMP").on("change", MPSelect);
            //$("#ddlDiv").on("change", DivSelect);





            $(function () {
                var dropDownMonitor = new iiisweb.UCDropDownMonitor(iiisweb.pubsub.publish, iiisweb.dataManager.sendRequest);
                dropDownMonitor.initialize();
            });
        });

        // dropdown event handler
        //$("#gridDDLDiv").on("change", function () {
        //    //Your Code  
        //    //Use .on instead of bind if you are using JQuery 1.7.x or higher   
        //    //http://api.jquery.com/on/ 

        //    $.ajax({
        //        "url": "/Controller/Action/" + $("#division").val(),
        //        "type": "get",
        //        "dataType": "json",
        //        "success": function (data) {
        //            //Your code here
        //            //data should be your json result
        //        }
        //    });
        //});
        //
        //$("#grid").on("rowSelect", function () {

        //    $("grid").getSelected().val()

        //    $.ajax({ url: "Home/Save", type: "POST", data: { reading: reading, readingID:    } })
        //        .done(function () {
        //            grid.reload();
        //            $("#readingModal").modal("hide");
        //        })
        //        .fail(function () {
        //            alert("Unable to save.");
        //            $("#readingModal").modal("hide");
        //        });

        //});

       
    </script>

</head>
<body>

    <div class="container readings">
        <h2>How to use jQuery Grid with ASP.NET MVC</h2>
        <br />
        <div class="row">
            <div class="col-md-3">

                <select id="ddlDiv" class="size-200" data-url="/Reading/ReadingList">
                    @*<option value="-1">Please select a division</option>
                    @For Each division As KeyValuePair(Of String, String) In Model.Divisions
                        @<option value="@division.Key">@division.Value</option>
                    Next*@
                </select>

            </div>
            <div class="col-md-3">

                <select id="ddlFeats" class="size-200" data-url="/Reading/ReadingList">
                    @*<option value="-1">Please select a division</option>
                @For Each division As KeyValuePair(Of String, String) In Model.Divisions
                    @<option value="@division.Key">@division.Value</option>
                Next*@
                </select>

            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="input-group">
                    <input type="text" id="search" class="form-control" placeholder="Search for...">
                    <span class="input-group-btn">
                        <button type="button" id="btnSearch" class="btn btn-default">Go!</button>
                    </span>
                </div>
            </div>
            <div class="col-md-9">
                <button type="button" id="btnAddReading" class="btn btn-default pull-right">Add New Reading</button>
            </div>
        </div>
        <br />
        <table id="grid" data-source="@Url.Action("ReadingList", "Reading")"></table>
        
    </div>
    
    @*<div class="table" id="grid" data-source="@Url.Action("ReadingList", "Reading")">
        <table id="grid"></table>

    </div>*@
            <!-- Modal -->
            <div class="modal fade" id="readingModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="false">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">Reading</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <input type="hidden" id="INSP_ID" />
                                <div class="form-group">
                                    <label for="Milepost">Milepost</label>
                                    <input type="text" class="form-control" id="Milepost" placeholder="Enter Milepost">
                                </div>
                                <div class="form-group">
                                    <label for="UC_Read_Date">Reading Date</label>
                                    <input type="text" class="form-control" id="UC_Read_Date" placeholder="Enter Reading Date">
                                </div>
                                <div class="form-group">
                                    <label for="Inspect_Date">Inspection Date</label>
                                    <input type="text" class="form-control" id="Inspect_Date" placeholder="Enter Inspection Date">
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <button type="button" id="btnSave" class="btn btn-primary">Save</button>
                        </div>
                    </div>
                </div>
            </div>

    @*<script src="~/Scripts/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="~/Scripts/bootstrap.min.js" type="text/javascript"></script>*@

    @*<script src="~/Scripts/iiisweb/iiisweb.utils.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.events.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.pubsub.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/iiisweb.data.js" type="text/javascript"></script>
    <script src="~/Scripts/grid_tvo-0.0.1.js" type="text/javascript"></script>
    <script src="~/Scripts/iiisweb/underclearance.js" type="text/javascript"></script>*@
</body>
</html>
