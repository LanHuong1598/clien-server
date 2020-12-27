/// <reference path="config.js" />
/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'vi';
    // config.uiColor = '#AADC6E';
    config.filebrowserBrowseUrl = '/Areas/Admin/Content/plugins/ckfinder/ckfinder.html',
        config.filebrowserUploadUrl = '/Areas/Admin/Content/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
        config.filebrowserWindowWidth = '1000',
        config.filebrowserWindowHeight = '700'
};