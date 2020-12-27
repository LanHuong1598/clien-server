(function ($, google, window, document, undefined) {

    var pluginName = "addressLookup",
        defaults = {
            "namespace": "al",
            "input": "#address-lookup"
        };

    function Plugin(element, options) {
        this.element = element;
        this.options = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype.init = function () {

        var selectors = {},
            name = this.options.namespace,
            components = ["results", "address", "link"];

        // populate the components for easy access
        for (var i = 0; i < components.length; i++) {
            component = components[i];
            selectors[component] = {};
            selectors[component].name = name + "-" + component;
            selectors[component].cls = "." + selectors[component].name;
        };
        // html vars
        var selector = this.options.input,
            container = this.element;

        // update the input box with gmap address
        var updateInput = function (e) {
            e.preventDefault();
            $(selector).val($(this).text());
            clear();
        }

        // remove all addresses
        var clear = function () {
            $(selectors.address.cls).remove();
        }

        // event binding functions
        var events = {
            bind: function (cls, callback) {
                $(cls).on("click", callback);
            },
            unbind: function (cls) {
                $(cls).unbind("click");
            }
        };

        // callback to handle data from google
        var handle = function (addresses) {
            var $form = $(container).find(selectors.results.cls);

            if (!$form.length) {
                $(container).append('<div class="' + selectors.results.name + '">')
            }

            // unbind any click events, then discard elements
            events.unbind(selectors.link.cls);
            clear();

            // add new elements
            if (addresses != null) {
                for (var i = 0; i < addresses.length; i++) {
                    address = addresses[i]
                    $form.append([
                    '<div class="', selectors.address.name, '">',
                        '<a class="', selectors.link.name, '" href="#">',
                            address.formatted_address,
                        '</a>',
                    '</div>'
                    ].join(''));
                };
            }
            // bind the new elements
            events.bind(selectors.link.cls, updateInput);
        }

        // google geocoder
        var geocoder = new google.maps.Geocoder();

        // listen for keyboard input
        $(selector).on("keyup", function (d) {
            var query = $(this).val();
            geocoder.geocode({ address: query }, handle);
        });

    }

    $.fn[pluginName] = function (options) {
        return new Plugin(this, options);
    }


})(jQuery, google, window, document);