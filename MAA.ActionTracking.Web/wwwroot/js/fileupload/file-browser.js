/// <reference path="../common/web-api-client.js" />
/// <reference path="../common/api-endpoints.js" />

(function (window) {

    function FileController() {

        let vm = this;

        vm.SelectFilesButton = $(".btn-file-select");
        vm.Loader = $('<div class="overlay-loader" id="overlay-loader"><div class="sk-fading-circle"><div class="sk-circle1 sk-circle"></div><div class="sk-circle2 sk-circle"></div><div class="sk-circle3 sk-circle"></div><div class="sk-circle4 sk-circle"></div><div class="sk-circle5 sk-circle"></div><div class="sk-circle6 sk-circle"></div><div class="sk-circle7 sk-circle"></div><div class="sk-circle8 sk-circle"></div><div class="sk-circle9 sk-circle"></div><div class="sk-circle10 sk-circle"></div><div class="sk-circle11 sk-circle"></div><div class="sk-circle12 sk-circle"></div></div></div>');
        vm.DeleteButton = $(".delete-button");
        //vm.DeleteThisFileButton = $('.file-library-inline-delete');

        this.Initialize = function () {
            //vm.LoadFiles(window.Folder);
            vm.SelectFilesButton.off("click").on("click", function (e) {
                e.preventDefault();
                window.AttachmentModal.modal("hide");
                let selectedFiles = _.map($(".file-content.selected"),
                    function (file) {
                        return $(file).data("file");
                    });
                //Raise file selected event with a list of selected files
                var editEvent = new CustomEvent('onFilesSelect', { bubbles: true, detail: selectedFiles });
                // Dispatch the event
                window.AttachmentModal.get(0).dispatchEvent(editEvent);
            });

            vm.DeleteButton.off("click").on("click", function (e) {
                e.preventDefault();

                if (vm.DeleteButton.hasClass("disabled")) //No file selected
                    return false;

                let lstFilesToBeDeleted = _.map($(".file-content.selected"), function (v,i) {
                    return $(v).data('file');
                });

                console.log(lstFilesToBeDeleted);
                WebApiClient.Delete(ApiEndpoints.Files.DeleteMultiple, { files: lstFilesToBeDeleted }, function (data) {
                    console.log(data);

                    $(".file-content.selected").fadeOut();
                    vm.EnableDisableDelButton();
                });

                //Raise file deletion event with a list of deleted files
                var deleteEvent = new CustomEvent('onFilesDeleted', { bubbles: true, detail: deletedFiles });
                // Dispatch the event
                window.AttachmentModal.get(0).dispatchEvent(deleteEvent);

            });
            

        };
        this.EnableDisableDelButton = function () {
            if ($(".file-content.selected").length > 0) {
                vm.DeleteButton.removeClass("disabled");
            }
            else {
                vm.DeleteButton.addClass("disabled");
            }
        }
        
        this.LoadFiles = function (folder,selected,tabToShowByDefault) {

            $("#files").append(vm.Loader);
            vm.Loader.show();

            WebApiClient.Get(ApiEndpoints.Files.Get + folder, null, function (response) {


                let htmlImages = '', htmlDocuments = '', htmlAudios = '', htmlVideos = '',htmlDartican='';
                let countImages = 0, countVideos = 0, countDocuments = 0, countAudios = 0,countDartican=0;
                _.each(response.files, function (f) {
                    let markup = '<li class="file-content" ' + (_.contains(selected, f.name) ? "selected" : "") + '" data-file="' + f.name + '" data-path="' + f.url + '"><i class="fa fa-trash-alt font-size-10 text-danger file-library-inline-delete" style="display:none;"></i><a href="#!"><i class="fa ' + vm.GetIcon(f) + '"></i><p>' + f.name + '</p></a></li>';
                 //htmlImages += '<div class="col-md-3 file-content ' + (_.contains(selected, f.name) ? "selected" : "") + '" data-file="' + f.name + '" data-path="' + f.url + '"><i class="fa-8x mdi ' + vm.GetIcon(f) + '"></i><span>' + f.name + '</span></a></div>';

                    if (vm.IsImage(f)) {
                        htmlImages += markup;
                        countImages++;
                    }
                    else if (vm.IsAudio(f)) {
                        htmlAudios += markup;
                        countAudios++;
                    }
                    else if (vm.IsVideo(f)) {
                        htmlVideos += markup;
                        countVideos++;
                    }
                    else if (vm.IsDartican(f)) {
                        htmlDartican += markup;
                        countDartican++;
                    }
                    else {
                        htmlDocuments += markup;
                        countDocuments++;
                    }

                });

                $("#files-images ul").html('').append(htmlImages);
                $("#count-images").text(countImages);
                $("#files-documents ul").html('').append(htmlDocuments);
                $("#count-documents").text(countDocuments);
                $("#files-videos ul").html('').append(htmlVideos);
                $("#count-videos").text(countVideos);
                $("#files-audios ul").html('').append(htmlAudios);
                $("#count-audios").text(countAudios);
                $("#files-dartican ul").html('').append(htmlDartican);
                $("#count-dartican").text(countDartican);

                if (tabToShowByDefault) {
                    //window.setTimeout(function () {
                        $(tabToShowByDefault).trigger('click');
                   // }, 200);
                }

                vm.Loader.hide();
                $(".file-content").off("dblclick").on("dblclick", function (event) {
                    $(".file-content.selected").removeClass("selected");
                    $(event.target).closest("li").addClass("selected");
                    vm.SelectFilesButton.trigger("click");
                    console.log(event.target);
                });
                $(".file-library-inline-delete").off("click").on("click", function (e) {
                     e.preventDefault();

                    let lstFilesToBeDeleted = _.map($(e.target).closest("li"), function (v, i) {
                        return $(v).data('file');
                    });
                    WebApiClient.Delete(ApiEndpoints.Files.DeleteMultiple, { files: lstFilesToBeDeleted }, function (data) {
                        console.log(data);

                        $(e.target).closest("li").fadeOut();
                        vm.EnableDisableDelButton();
                    });

                    //Raise file deletion event with a list of deleted files
                    var deleteEvent = new CustomEvent('onFilesDeleted', { bubbles: true, detail: deletedFiles });
                    // Dispatch the event
                    window.AttachmentModal.get(0).dispatchEvent(deleteEvent);
                });

                $(".file-content").off("click").on("click", function (e) {
                    if (!$(e.target).hasClass("file-library-inline-delete")) {
                        if ($(this).hasClass("selected"))
                            $(this).removeClass("selected");
                        else
                            $(this).addClass("selected");

                        vm.EnableDisableDelButton();
                    }
 
                });
                vm.EnableDisableDelButton();
            }, null);
        };

        this.GetIcon = function (file) {

            if (vm.IsImage(file))
                return 'fa-file-image';
            else if (vm.IsAudio(file))
                return 'fa-file-audio';
            else if (vm.IsVideo(file))
                return 'fa-file-video';
            else if (s(file.name).toLowerCase().endsWith(".xls") || s(file.name).toLowerCase().endsWith(".xlsx"))
                return 'fa-file-excel';
            else if (s(file.name).toLowerCase().endsWith(".doc") || s(file.name).toLowerCase().endsWith(".docx"))
                return 'fa-file-word';
            else if (s(file.name).toLowerCase().endsWith(".pdf"))
                return 'fa-file-pdf';
            else if (s(file.name).toLowerCase().endsWith(".xml"))
                return 'fa-file-xml';
            else if (s(file.name).toLowerCase().endsWith(".html"))
                return 'fa-file-html';
            else if (s(file.name).toLowerCase().endsWith(".txt"))
                return 'fa-file-text';
            else if (s(file.name).toLowerCase().endsWith(".dartican"))
                return 'fa-file-code';
            else
                return 'fa-file-documents';

        };

        this.IsImage = function (file) {
            return s(file.name).toLowerCase().endsWith(".png") || s(file.name).toLowerCase().endsWith(".jpg") || s(file.name).toLowerCase().endsWith(".gif") || s(file.name).toLowerCase().endsWith(".jpeg");
        };

        this.IsAudio = function (file) {
            return s(file.name).toLowerCase().endsWith(".mp3") || s(file.name).toLowerCase().endsWith(".wma") || s(file.name).toLowerCase().endsWith(".wav");
        };

        this.IsVideo = function (file) {
            return s(file.name).toLowerCase().endsWith(".mp4") || s(file.name).toLowerCase().endsWith(".mpeg");
        };

        this.IsDartican = function (file) {
            return s(file.name).toLowerCase().endsWith(".dartican");
        };
    }

    //Export globally
    window.FileBrowser = new FileController();
    window.FileBrowser.Initialize();

}(window));