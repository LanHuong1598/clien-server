"use strict";
const utils = {
    loading: () => {
        const html = `<div class="fade-loading" id='loading'><div class="lds-ring"><div></div><div></div><div></div><div></div></div></div>`;
        $("body").append(html);
    },
    done: () => {
        $("#loading").remove();
    }
};