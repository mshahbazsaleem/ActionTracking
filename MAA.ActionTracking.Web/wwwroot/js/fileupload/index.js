// Write your Javascript code.


//window.UploadForm = $('#fileupload');

//$(function () {

//    UploadForm.fileupload({
//        dataType: 'json',
//        //formData: { type: UploadForm.data("for") },
//        change: function (e, data) {

//            UploadForm.find(".fileupload-progress").addClass("show");

//            if (UploadForm.data("show-upload-button")) {

//                UploadForm.find("button.start").removeClass("hidden");
//            }
//        }
//    }).bind('fileuploaddone', function (e, data) {

//        UploadForm.get(0).dispatchEvent(new CustomEvent('onUploadSuccess', { bubbles: true, detail: data }));

//    }).bind('fileuploadfail', function (e, data) {
//        UploadForm.get(0).dispatchEvent(new CustomEvent('onUploadFail', { bubbles: true, detail: data }));
//    });

//    $('.btn-upload').click(function () {
//        $('#fileupload button.start').click();
//    });

//});

////$('#fileupload').addClass('fileupload-processing');



window.UploadForms = $('.file-upload');

$(function () {

    //Setup each upload form here, actual upload trigger
    //is in the respective .js file of view having upload functionality
    //e.g for attachment
    //
    //$('.btn-upload').click(function () {
    //    $('#fileupload button.start').click();
    //});

    UploadForms.each(function () {
        let UploadForm = $(this);
        let dropZoneElementId = UploadForm.data("dropzone");
        let fileUpload = null;

        if (dropZoneElementId === '') {
            fileUpload = UploadForm.fileupload({
                dataType: 'json',
                autoUpload:true,
                //formData: { type: UploadForm.data("for") },
                change: function (e, data) {
                   
                    //UploadForm.find(".fileupload-progress").addClass("show");

                    //if (UploadForm.data("show-upload-button")) {

                    //    UploadForm.find("button.start").removeClass("hidden");
                    //}

                    UploadForm.find("#upload-control").removeClass("hidden");
                }
            });
        }

        else
        {
            fileUpload = UploadForm.fileupload({
                dataType: 'json',
                dropZone: $('#' + dropZoneElementId),
                autoUpload: true,
                //formData: { type: UploadForm.data("for") },
                change: function (e, data) {

                    //UploadForm.find(".fileupload-progress").addClass("show");

                    //if (UploadForm.data("show-upload-button")) {

                    //    UploadForm.find("button.start").removeClass("hidden");
                    //}

                    UploadForm.find("#upload-control").removeClass("hidden");
                }
            });
        }

        fileUpload.data("count", 0);
        fileUpload.data("processed", 0);

        fileUpload
            .bind('fileuploadadd', function (e, data) {
               
                UploadForm.get(0).dispatchEvent(new CustomEvent('onUploadFileSelected', { bubbles: true, detail: data.files }));

                fileUpload.data("count", parseInt(fileUpload.data("count")) + 1);

                //Show first file name 
            })
            .bind('fileuploadprogress', function (e, data) {

                var progress = parseInt(data.loaded / data.total * 100, 10);

                UploadForm.get(0).dispatchEvent(new CustomEvent('onUploadProgress', { bubbles: true, detail: progress }));

            })
            //.bind('fileuploadprogressall', function (e, data) { console.log(data); })
            //.bind('fileuploadstart', function (e) { console.log(e); })
            .bind('fileuploaddone', function (e, data) {

                fileUpload.data("processed",parseInt(fileUpload.data("processed")) + 1);

                let count = parseInt(fileUpload.data("count"));
                let processed = parseInt(fileUpload.data("processed"));

                if (count === processed)
                    UploadForm.get(0).dispatchEvent(new CustomEvent('onFilesUploaded', { bubbles: true, detail: data }));

                UploadForm.get(0).dispatchEvent(new CustomEvent('onUploadSuccess', { bubbles: true, detail: data }));

            }).bind('fileuploadfail', function (e, data) {

                UploadForm.get(0).dispatchEvent(new CustomEvent('onUploadFail', { bubbles: true, detail: data }));

            });
    });


    //$('.btn-upload').click(function () {
    //    $('#fileupload button.start').click();
    //});

});

