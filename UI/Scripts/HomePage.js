var fileResults;         // to save the latest ajax posted results 
var filesPathArray = []; // used to save selected files paths to download 
var filesNameArray = []; // used to save selected files names to download 
var files;               //used in prepareUpload Func

$(document).ready(function () {
    (document.location.href).replace("#", '');

    var ParentPath = $("#Map").attr("currentpath");

    $.ajax({
        type: "POST",
        url: "../../Defualt.aspx/GetDirctoryInfo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ 'parentPath': ParentPath, 'isLevelList': false }),
        success: function (result, xhr, request) {

            OnSuccess(result.d);
            fileResults = result.d;
        }
    });




});

function OnSuccess(response) {

    var FileData = JSON.parse(response);
    $(".ImageBlocks-container").empty();
    for (i = 0; i < FileData.length; i++) {
        var ImagePath = FileData[i].LargeThumbnail;
        if ($('.ImageBlocks-container').hasClass('ListView')) { ImagePath = FileData[i].SmallThumbnail }
        if (FileData[i].Type == 0) {
            $(".ImageBlocks-container").append('\
  <li  class="ImageBlock" Type ="' + FileData[i].Type + '" id =' + FileData[i].ID + ' Path=' + FileData[i].Path + ' name="' + FileData[i].Name + '" md5="' + FileData[i].MD5 + '" Parentpath="' + FileData[i].ParentPath + '" Ext="' + FileData[i].Extension + '">\
        <a title="' + FileData[i].Name + '" href="#" >\
            <img class="thumbnail img-responsive" id=' + FileData[i].ID + ' src=' + ImagePath + '>\
               <div class="container_Tools" id="Tools">\
                	<nav class="nav__menu_icons">\
                    	<ul>\
                        	    <li class="Left-side">\
                            	    <section title="squaredThree">\
                                	    <input type="checkbox" value="None" class="checkBox" name="check"  />\
                                        <label for="squaredThree"></label>\
                                    </section>\
                                </li>\
                                <li>\
                            	      <i class="fa fa-trash-o DeleteFile" aria-hidden="true"  ></i>\
                                <li>\
                                      <i class="fa fa-cloud-download downloadFile" aria-hidden="true"></i>\
                                </li>\
                                <li>\
                                      <i class="fa fa-pencil-square-o editFile" aria-hidden="true"></i>\
                                </li>\
                                <li>\
                                      <i class="fa fa-info-circle" aria-hidden="true"></i>\
                                </li>\
                        </ul>\
                    </nav>\
               </div>\
</a>\
          <div calss="info-container" id="info-container"><i class="fa fa-file-image-o" aria-hidden="true"></i>&nbsp;' + FileData[i].Name + '</div>\
  </li>'
            );
        } else {
            $(".ImageBlocks-container").append('\
  <li class="Folder-Block" Type ="' + FileData[i].Type + '" id =' + FileData[i].ID + ' Path=' + FileData[i].Path + ' name="' + FileData[i].Name + '" md5="' + FileData[i].MD5 + '" Parentpath="' + FileData[i].ParentPath + '" Ext="' + FileData[i].Extension + '">\
        <a  title="' + FileData[i].Name + '" href="#" >\
            <img class="thumbnail img-responsive bigFolder " id=' + FileData[i].ID + ' src="Uploads/FolderImage.png">\
               <div class="container_Tools" id="Tools">\
                	<nav class="nav__menu_icons">\
                    	<ul>\
                        	    <li class="Left-side">\
                            	    <section title=".squaredThree">\
                                	    <input type="checkbox" value="None" class="checkBox" name="check"  />\
                                        <label for="squaredThree"></label>\
                                    </section>\
                                </li>\
                                <li>\
                            	      <i class="fa fa-trash-o DeleteFolder" aria-hidden="true"  ></i>\
                                <li>\
                                      <i class="fa fa-cloud-download downloadFolder" aria-hidden="true"></i>\
                                </li>\
                                <li>\
                                      <i class="fa fa-pencil-square-o editFolder" data-toggle="modal" data-target="#RenameFolder" aria-hidden="true"></i>\
                                </li>\
                                <li>\
                                      <i class="fa fa-info-circle" aria-hidden="true"></i>\
                                </li>\
                        </ul>\
                    </nav>\
               </div>\
   </a>\
 <div calss="info-container" id="info-container"><i class="fa fa-folder fa"  aria-hidden="true"></i>&nbsp;' + FileData[i].Name + '</div>\
  </li>'
            );

        }
    }

    $(".DeleteFile").click(function () {
        var type = parseInt($(this).closest(".ImageBlock").attr('type'));
        var Name = $(this).closest(".ImageBlock").attr('name');
        var extension = $(this).closest(".ImageBlock").attr('ext');
        var ID = parseInt($(this).closest(".ImageBlock").attr('id'));
        var parentPath = $(this).closest(".ImageBlock").attr('Parentpath');
        var md5 = $(this).closest(".ImageBlock").attr('md5');
        if (confirm('Are you sure you want to  delete  ' + Name + ' from your storages  ?')) {
            $.ajax({
                type: "POST",
                url: "../../Defualt.aspx/DeleteFile",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'path': "", 'parentPath': parentPath, 'id': ID, 'md5': md5, 'type': type, 'fileName': Name, 'extension': extension }),
                success: function (result, xhr, request) {

                    $('#' + ID).remove();
                }

            });
        }


    });

    $('.DeleteFolder').click(function () {
        var Name = $(this).closest(".Folder-Block").attr('name');
        var parentPath = $(this).closest(".Folder-Block").attr('parentPath');
        var ID = parseInt($(this).closest(".Folder-Block").attr('id'));
        var path = Name
        var type = parseInt($(this).closest(".Folder-Block").attr('type'));

        if (confirm('Are you sure you want to  delete [ ' + Name + ' ] folder from your storage  ?')) {
            $.ajax({
                type: "POST",
                url: "../../Defualt.aspx/DeleteFile",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'path': path, 'parentPath': parentPath, 'id': ID, 'md5': null, 'type': type, 'fileName': Name, 'extension': null }),
                success: function (result, xhr, request) {

                    $('#' + ID).remove();
                }

            });
        }

    });

    $('.ImageBlock').dblclick(function () {
        var modalImg = document.getElementById("img01");
        var captionText = document.getElementById("bigImageModal-caption");
        modalImg.src = $(this).attr('path');
        captionText.innerHTML = $(this).attr('name');
        $('#bigImageModal').modal('toggle');
        $('.closeModal').click(function () {
            $('#bigImageModal').modal('hide');
        });
    });

    $(".bigFolder").click(function GetFolder() {
        var Name = $(this).closest(".Folder-Block").attr('name');
        var ParentPath = $(this).closest(".Folder-Block").attr("parentPath") + Name + '/';
        $("#Map").attr("currentpath", ParentPath);
        var RootPath = "";
        $.ajax({
            type: "POST",
            url: "../../Defualt.aspx/GetDirctoryInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ 'parentPath': ParentPath, 'isLevelList': false }),
            success: function (result, xhr, request) {

                OnSuccess(result.d);
                fileResults = result.d;
            }
        });
        if (ParentPath == "" + Name + '/') {

            var FolderString = '\
 <li class="Left-side list-group-item Map-Item hvr-skew-forward" path="' + ParentPath + '" >\
					 <i class="fa fa-arrow-circle-right" aria-hidden="true"><a href="#">' + Name + '</a>'
            $(".Map-ul").append(FolderString);


        } else {

            var FolderString = '\
                <li class="Left-side list-group-item Map-Item hvr-skew-forward" path="' + ParentPath + '" >\
						<i class="fa fa-arrow-circle-right" aria-hidden="true"></i>\<a href="#">' + Name + '</a>\
                 </li>'
            $(".Map-ul").append(FolderString);
        }
    });

    $(".addFolder").submit(function () {
        var folderName = $(".folderName")[0].value;
        var ParentPath = $("#Map").attr("currentpath");

        $.ajax({
            type: 'POST',
            url: "../../Defualt.aspx/AddFolder",
            data: JSON.stringify({ 'parentPath': ParentPath, 'folderName': folderName }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, xhr, request) {
                $("ImageBlocks-container").append('\
  <li class="Folder-Block"  Path=' + ParentPath + ' name="' + folderName + '">\
        <a  title="' + FileData[i].Name + '" href="#" >\
            <img class="thumbnail img-responsive bigFolder " id="" src="Uploads/FolderImage.png">\
               <div class="container_Tools" id="Tools">\
                	<nav id="nav__menu_icons">\
                    	<ul>\
                        	    <li class="Left-side">\
                            	    <section title=".squaredThree">\
                                	    <input type="checkbox" value="None" class="checkBox" name="check"  />\
                                        <label for="squaredThree"></label>\
                                    </section>\
                                </li>\
                                <li>\
                            	      <i class="fa fa-trash-o DeleteFile" aria-hidden="true"  ></i>\
                                <li>\
                                      <i class="fa fa-cloud-download downloadFile" aria-hidden="true"></i>\
                                </li>\
                        </ul>\
                    </nav>\
               </div>\
 <div calss="info-container" id="info-container"><i class="fa fa-folder fa"  aria-hidden="true"></i>&nbsp;' + folderName + '</div>\
        </a>\
  </li>')

            }

        });
        var folderName = "";
        var ParentPath = "";
    });

    $('input[type="checkbox"]').click(function () {

        if ($(this).is(':checked')) {
            $(".social-banner .Delete").css("display", "block");
            $(".social-banner .Downloadbutton").css("display", "block");
            $('.container_Tools').css('height', '25px'), ('bottom', '90%');

            var Path = $(this).closest(".ImageBlock").attr('path');
            var Name = $(this).closest(".ImageBlock").attr('name') + $(this).closest(".ImageBlock").attr('Ext');
            var id = $(this).closest(".ImageBlock").attr('id');

            $("#" + id).css("background", "#9aa0a5");
            filesNameArray.push(Name);
            filesPathArray.push(Path);

        } else {
            var id = $(this).closest(".ImageBlock").attr('id');
            var Path = $(this).closest(".ImageBlock").attr('path');
            var Name = $(this).closest(".ImageBlock").attr('name');
            $("#" + id).css("background", "");
            filesPathArray.pop(Path);
            filesNameArray.pop(Name);

        } if (filesPathArray == 0) {
            $('.container_Tools').css('height', ''), ('bottom', '');
            $(".social-banner .Downloadbutton").css("display", "none");
            $(".social-banner .Delete").css("display", "none");


        }



    });

    $(".Map-Item").click(function UpdateMap() {
        var Path = $(this).attr('path');
        $(".progress-bar").css("width", 0 + "%");
        if (Path == '') {

            $(this).nextAll('li').remove();

        } else {
            $(this).nextAll('li').remove();
        }

        $.ajax({
            type: "POST",
            url: "../../Defualt.aspx/GetDirctoryInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ 'parentPath': Path, 'isLevelList': false }),
            success: function (result, xhr, request) {

                OnSuccess(result.d);
                fileResults = result.d;
            }
        });

    });

    $('.downloadFile').click(function () {
        var Path = $(this).closest(".ImageBlock").attr('path');
        var Name = $(this).closest(".ImageBlock").attr('name');
        var link = document.createElement("a");
        link.download = Name;
        link.href = Path;
        link.click();
    });

    $('.Downloadbutton').click(function () {
        DownloadZip();

    });

    $('.downloadFolder').click(function () {
        var Name = $(this).closest(".Folder-Block").attr('name');
        var ParentPath = $("#Map").attr("currentpath");
        $.ajax({
            type: "POST",
            url: "../../Defualt.aspx/GetDirctoryInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ 'parentPath': Name, 'isLevelList': true }),
            success: function (result, xhr, request) {

                DownloadFolder(result.d);

            }
        });
    });

    $('.editFolder').click(function () {
        var folderName = $(this).closest(".Folder-Block").attr('name');
        var folderID = parseInt($(this).closest(".Folder-Block").attr('id'));
        $('.RenameFolder').submit(function () {
            var folderNewName = $(".folderNewName")[0].value;
            $.ajax({
                type: "POST",
                url: "../../Defualt.aspx/RenameFolder",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'Name': folderName, 'fileNewName': folderNewName, 'ID': folderID }),
                success: function (result, xhr, request) {
                    $("#" + folderID).attr('name').val(folderNewName);
                }

            });

        });


    });
}

function prepareUpload(event) {
    $(".progress-bar").css({ 'display': '' });
    files = event.target.files
    if (files.size > 1048576) {
        uploadChunks();
    } else {
        uploadFiles();
    }

}

function uploadFiles() {
    var formData = new FormData();
    var ParentPath = $("#Map").attr("currentpath");
    $.each(files, function (key, value) {
        value.ParentPath = ParentPath;
        formData.append('file[' + key + ']', value);
    });
    $('#parentPathHidden').val(ParentPath);
    $.ajax({
        headers: { 'parentPath': ParentPath },
        type: 'POST',
        url: 'FileUploadHandler.ashx',
        data: formData,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();

            // Upload progress
            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
                    //Do something with upload progress
                    $(".progress-bar").css("width", 0 + "%");
                    $(".progress-bar").css("width", (Math.floor(evt.loaded / evt.total * 100) - 10) + "%");

                }
            }, false);

            // Download progress
            xhr.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
                    // Do something with download progress
                }
            }, false);

            return xhr;
        },

        contentType: false,
        processData: false,
        success: function (result, xhr, request) {
            var FileData = JSON.parse(result);


            for (i = 0; i < FileData.length; i++) {

                var ImagePath = FileData[i].LargeThumbnail;

                if ($('.ImageBlocks-container').hasClass('ListView')) { ImagePath = FileData[i].SmallThumbnail }
                if (FileData[i].Type == 0) {
                    $(".ImageBlocks-container").append('\
  <li  class="ImageBlock" Type ="' + FileData[i].Type + '" id =' + FileData[i].ID + ' Path=' + FileData[i].Path + ' name="' + FileData[i].Name + '" md5="' + FileData[i].MD5 + '" Parentpath="' + FileData[i].ParentPath + '" Ext="' + FileData[i].Extension + '">\
        <a title="' + FileData[i].Name + '" href="#" >\
            <img class="thumbnail img-responsive" id=' + FileData[i].ID + ' src=' + ImagePath + '>\
               <div class="container_Tools" id="Tools">\
                	<nav class="nav__menu_icons">\
                    	<ul>\
                        	    <li class="Left-side">\
                            	    <section title="squaredThree">\
                                	    <input type="checkbox" value="None" class="checkBox" name="check"  />\
                                        <label for="squaredThree"></label>\
                                    </section>\
                                </li>\
                                <li>\
                            	      <i class="fa fa-trash-o DeleteFile" aria-hidden="true"  ></i>\
                                <li>\
                                      <i class="fa fa-cloud-download downloadFile" aria-hidden="true"></i>\
                                </li>\
                                <li>\
                                      <i class="fa fa-pencil-square-o editFile" aria-hidden="true"></i>\
                                </li>\
                                <li>\
                                      <i class="fa fa-info-circle" aria-hidden="true"></i>\
                                </li>\
                        </ul>\
                    </nav>\
               </div>\
</a>\
          <div calss="info-container" id="info-container"><i class="fa fa-file-image-o" aria-hidden="true"></i>&nbsp;' + FileData[i].Name + '</div>\
  </li>'
                    );

                }
            } $(".progress-bar").css("width", 100 + "%");

        },
        error: function (error) {
            alert("Upload Faild would you please try again");

        }
    });
}

function uploadChunks() {
    var formData = new FormData();
    var ParentPath = $("#Map").attr("currentpath");
    $.each(files, function (key, value) {
        value.ParentPath = ParentPath;
        formData.append('file[' + key + ']', value);
    });
    $('#parentPathHidden').val(ParentPath);
    var loaded = 0;
    var step = 1048576//1024*1024; size of one chunk
    var total = formData.size;  // total size of file
    var start = 0;          // starting position
    var reader = new FileReader();
    var blob = file.slice(start, step); //a single chunk in starting of step size
    reader.readAsBinaryString(blob);   // reading that chunk. when it read it, onload will be invoked

    reader.onload = function (e) {
        var d = { file: reader.result }
        $.ajax({
            headers: { 'parentPath': ParentPath },
            url: 'FileUploadHandler.ashx',
            type: "POST",
            data: d                     // d is the chunk got by readAsBinaryString(...)
        }).done(function (r) {           // if 'd' is uploaded successfully then ->

            loaded += step;                 //increasing loaded which is being used as start position for next chunk
            $(".progress-bar").css("width", (loaded / total) * 100);

            if (loaded <= total) {            // if file is not completely uploaded
                blob = file.slice(loaded, loaded + step);  // getting next chunk
                reader.readAsBinaryString(blob);        //reading it through file reader which will call onload again. So it will happen recursively until file is completely uploaded.
            } else {                       // if file is uploaded completely
                loaded = total;            // just changed loaded which could be used to show status.
            }
        })
    };
}



function DownloadFolder(response) {
    var FileData = JSON.parse(response);

    for (i = 0; i < FileData.length; i++) {
        if (FileData[i].Type == 0) {
            filesPathArray.push(FileData[i].Path)
            filesNameArray.push(FileData[i].Name + FileData[i].Extension)
        }
    }
    DownloadZip();
}

function DownloadZip() {
    var zip = new JSZip();
    var count = 0;
    var zipFilename = "IData.zip";
    var urls = filesPathArray;

    urls.forEach(function (url, index, total) {
        var filename = filesNameArray[index];
        // loading a file and add it in a zip file
        JSZipUtils.getBinaryContent(url, function (err, data) {

            if (err) {
                throw err; // or handle the error
            }
            zip.file(filename, data, { binary: true });
            count++;
            if (count == urls.length) {
                zip.generateAsync({ type: 'blob' }).then(function (content) {
                    saveAs(content, zipFilename);
                });
            }
        });

    });

}
$('#file-input').on('change', prepareUpload);

$('.GridView').click(function () { $('.ImageBlocks-container').removeClass('ListView'); });

$('.ListView').click(function () { $('.ImageBlocks-container').addClass('ListView'); });

function SearchBar() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    ul = document.getElementById("ImagesUL");
    li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";

        }
    }
}