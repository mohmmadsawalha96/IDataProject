<%@ Page Title="" Language="C#" MasterPageFile="~/IData.Master" AutoEventWireup="true" CodeBehind="Defualt.aspx.cs" Inherits="IData.Home" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Scripts/jquery-3.2.1.min.js"></script>
    <link href="Content/css/ProgressBar.css" rel="stylesheet" />
    <link href="Content/css/Home.css" rel="stylesheet" />

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">

                <form action="FileUploadHandler.ashx" method="post">
                    <input type="hidden" name="pp" id="pp" value="root" runat="server" />
                </form>
                <nav class="social-banner navbar-fixed-top navbar navbar-inverse navTools">
                    <ul>
                        <li class="list-group-item Delete ">
                            <i class="fa fa-trash-o hvr-icon-rotate" aria-hidden="true"></i>

                        </li>
                        <li class="list-group-item">
                            <div class="image-upload ">
                                <label for="file-input">
                                    <i class="fa fa-cloud-upload hvr-icon-up" aria-hidden="true"></i>

                                </label>

                                <input id="file-input" type="file" multiple />
                            </div>


                        </li>
                        <li class="list-group-item Downloadbutton ">

                            <i class="fa fa-cloud-download hvr-icon-down" aria-hidden="true"></i>
                        </li>
                        <li class="list-group-item">

                            <i class="fa fa-folder fa hvr-icon-up" data-toggle="modal" data-target="#addFolderModel" aria-hidden="true"></i>
                        </li>
                        <li class=" list-group-item GridView ">
                            <i class="fa fa-th-large hvr-icon-pulse" aria-hidden="true"></i>
                        </li>
                        <li class=" list-group-item ListView ">
                            <i class="fa fa-list hvr-icon-pulse" aria-hidden="true"></i>

                        </li>


                    </ul>

                </nav>
                <nav class=" Map-bar social-banner navbar navbar-inverse navbar-fixed-top" currentpath="" id="Map">
                    <ul class="Map-ul">
                        <li class="Left-side list-group-item Map-Item" path="">
                            <a href="#"><i class="fa fa-home " aria-hidden="true"></i></a>
                            </i></li>
                    </ul>
                </nav>
                <div class="progress progress-striped">
                    <div class="progress-bar progress-bar-custom" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 0%;">
                        <span class="sr-only">100% Complete</span>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12 col-xs-12 col-imageHolder container pre-scrollable scrollbar" id="scoroll-style-14">

                    <ul class="ImageBlocks-container ImageUL container">
                    </ul>
                </div>

            </div>

            <div class="modal fade" id="addFolderModel" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Folder Name</h4>
                        </div>
                        <div class="modal-body">
                            <form class="addFolder">
                                <input type="text" name="folderName" class="folderName">
                                <input type="submit" value="Add Folder">
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>
            <div class="modal fade" id="RenameFolder" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Folder Name</h4>
                        </div>
                        <div class="modal-body">
                            <form class="RenameFolder">
                                <input type="text" name="folderName" class="folderNewName">
                                <input type="submit" value="Rename Folder">
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>
            <div id="bigImageModal" class="bigImageModal">
                <span class="closeModal">&times;</span>
                <div class="bigImageBlock">
                    <div class="bigImageView">
                        <img class="bigImageModal-content" id="img01" src="" />
                        <div id="bigImageModal-caption"></div>
                    </div>
                </div>
            </div>
            <div tabindex="-1" class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button class="close" type="button" data-dismiss="modal">×</button>
                            <h3 class="modal-title">Heading</h3>
                        </div>
                        <div class="modal-body">
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>




        </div>
    </div>

    <%--   <div class="progress progress-striped">
        <div class="progress-bar progress-bar-custom" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 0%;">
            <span class="sr-only">100% Complete</span>
        </div>
    </div>--%>
</asp:Content>
