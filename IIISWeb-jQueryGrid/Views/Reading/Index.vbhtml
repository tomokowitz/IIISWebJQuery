@ModelType IIISWeb_jQueryGrid.Models.ReadingModel
@Code
    ViewData("Title") = "Index"

End Code

@section Scripts
    <script src="~/Scripts/CascadingDropDownSample.js"></script>
End Section

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
    <link href="~/Content/grid-0.4.3.min.css" rel="stylesheet" type="text/css">

    @*<script src="~/Scripts/tether/tether.min.js" type="text/javascript"></script>*@
    <script src="~/Scripts/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="~/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/iiisweb.utils.js")"></script>
    <script src="@Url.Content("~/Scripts/iiisweb.events.js")"></script>
    <script src="@Url.Content("~/Scripts/iiisweb.pubsub.js")"></script>
    <script src="@Url.Content("~/Scripts/iiisweb.data.js")"></script>
    <script src="~/Scripts/grid_tvo-0.0.1.js"></script>
    <script src="~/Scripts/iiisweb/underclearance.js"></script>
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

        //function DivSelect() {
        //    $.ajax({
        //        "url": "/Controller/Action/" + $("#division").val(),
        //        "type": "get",
        //        "dataType": "json",
        //        "success": function (data) {
        //            //Your code here
        //            //data should be your json result
        //        }
        //    });
        //}
        $(document).ready(function () {
            //grid = $("#grid").grid({
            //    dataKey: "INSP_ID",
            //    uiLibrary: "bootstrap",
            //    columns: [
            //        { field: "Milepost", width: 100, sortable: true },
            //        { field: "INSP_ID", sortable: true },
            //        { field: "UC_Read_Date", title: "Reading Date", sortable: true },
            //        { field: "Inspect_Date", title: "Inspection Date", sortable: true }
            //        //,
            //        //{ title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            //        //{ title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": Remove }  }
            //    ],
            //    pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
            //});
            // need to populate dropdowns here???


            $("#btnAddReading").on("click", Add);
            $("#btnSave").on("click", Save);
            $("#btnSearch").on("click", Search);
            // add dropdown and other grid events registration here???
            //$("#gridDDLMP").on("change", MPSelect);
            //$("#gridDDLDiv").on("change", DivSelect);

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

        //$(function () {
        //    var dropDownMonitor = new iiisweb.UCDropDownMonitor(iiisweb.pubsub.publish, iiisweb.dataManager.sendRequest);
        //    dropDownMonitor.initialize();
        //});
    </script>

</head>
<body>

    <div class="container readings">
        <h2>How to use jQuery Grid with ASP.NET MVC</h2>
        <br />
        <div class="row">
            <div class="col-md-3">

                @*<select id="division" class="size-200">
                    <option value="-1">Please select a division</option>
                    @For Each division As KeyValuePair(Of String, String) In Model.Divisions
                        @<option value="@division.Key">@division.Value</option>
                    Next
                </select>*@

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
        <table id="grid" data-source="@Url.Action("Index", "Reading")"></table>
    </div>

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
</body>
</html>
