// based on http://jqueryui.com/autocomplete/#combobox

// TODO: Re-enable tooltips
function dnnAcCombobox_Init ($) {
    $.widget("dnn_ac.combobox", {

        _create: function() {
            this.wrapper = $("<div>")
                .addClass("dnn-ac-combobox")
                .insertAfter(this.element);
            this.element.hide();
            this._createAutocomplete();
            this._createShowAllButton();
        },
 
        _createAutocomplete: function() {
            var selected = this.element.children(":selected"),
            value = selected.val() ? selected.text() : "";
 
        this.input = $("<input>")
            .appendTo(this.wrapper)
            .val(value)
            .attr("title", "")
            .addClass("dnn-ac-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
            .autocomplete({
                delay: 0,
                minLength: 0,
                source: $.proxy(this, "_source")
            });
            /*
            }).tooltip({
                tooltipClass: "ui-state-highlight"
            });
            */

        this._on(this.input, {
                autocompleteselect: function(event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },
                autocompletechange: "_removeIfInvalid"
            });
        },
 
        _createShowAllButton: function() {
            var input = this.input,
            wasOpen = false;
     
            $("<a>")
                .attr("tabIndex", -1)
                // .attr("title", "Show All Items")
                // .tooltip()
                .appendTo(this.wrapper)
                .button({ label: "&#9207;" })
                .removeClass("ui-corner-all")
                .addClass("ui-corner-right dnnSecondaryAction dnn-ac-combobox-toggle")
                .mousedown(function() {
                    wasOpen = input.autocomplete("widget").is(":visible");
                })
                .click(function() {
                    input.focus();
                    // close if already visible
                    if ( wasOpen ) {
                      return;
                    }
                    // pass empty string as value to search for, displaying all results
                    input.autocomplete("search", "");
                });
        },
     
        _source: function( request, response ) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response( this.element.children("option").map(function() {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                      label: text,
                      value: text,
                      option: this
                    };
                }) 
            );
        },
     
        _removeIfInvalid: function( event, ui ) {
            // selected an item, nothing to do
            if (ui.item) {
              return;
            }
     
            // search for a match (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function() {
                if ( $(this).text().toLowerCase() === valueLowerCase ) {
                    this.selected = valid = true;
                    return false;
                }
            });
     
            // found a match, nothing to do
            if (valid) {
                return;
            }

            // select first (default) element
            var defaultElement = this.element.children("option").first();
            this.element.val(defaultElement.val());
            this.input.val(defaultElement.text());

            /*
            this.input.val("")
                .attr("title", value + " didn't match any item")
                .tooltip("open");
            this.element.val("");

            this._delay(function() {
                this.input
                    .tooltip("close")
                    .attr("title", "");
            }, 2500);
            */

            this.input.autocomplete("instance").term = "";
        },
     
        _destroy: function() {
            this.wrapper.remove();
            this.element.show();
        }
    });
}