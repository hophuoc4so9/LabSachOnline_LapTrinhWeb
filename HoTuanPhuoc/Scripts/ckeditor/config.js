/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
    config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
    config.fiCKEDITOR.editorConfiglebrowserImageBrowseUrl = '~/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
};
