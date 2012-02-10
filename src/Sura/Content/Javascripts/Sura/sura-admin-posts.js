$(document).ready(function () {
    if ($('#Tags')) {
        $('#Tags').tagInput();
    }

    var button = $('<a />').append($('<i />').addClass('icon-calendar')).click(function () {
        $('#ScheduleFor').focus();
    });
    
    var scheduler = $('#schedule-datetimepicker').prepend(button).hide();

    $('#ScheduleFor').datetimepicker({
        numberOfMonths: 2,
	    stepMinute: 10,
        dateFormat: 'dd/mm/yy',
        minDate: new Date()
    });

    if ($('#Availability').val() === 'Scheduled') {
        scheduler.show(); 
    }
    
    $('#Availability').change(function () {
        if ($(this).val() === 'Scheduled') {
            scheduler.show(); 
            $('#ScheduleFor').focus();
        } else {
            scheduler.hide();
        }
    });
});

(function ($) {
    $.fn.tagInput = function() {
        var input = $(this)
           , div = $('<div />').addClass('post-tags').insertAfter(input)
           , copy = $(this).clone().attr('id', '_Tags').attr('name', '_Tags').val('').appendTo(div)
           , tagList = $('<ul />').addClass('taglist').appendTo(div);

        input.prev().attr('for', '_Tags');
        input.hide();

        var _addToList = function(value) {
            var remove = $('<a />', { tabindex: '-1' }).addClass('remove').append($('<span />').html('[X]')).click(function() {
                $(this).parent().parent().remove();
            })

            var listTag = $('<span/>').html(value).append(remove)
             , listItem = $('<li/>').addClass('tag').html(listTag);
                
            tagList.append(listItem);
        };
        
        var _addTag = function (tag) {
            var value = $(tag).val();
            var tags = input.val().split(',');
            
            if ($.inArray(value, tags) === -1) {
                tags.push(value);
                input.val(tags.splice(tags[0] === '' ? 1 : 0, tags.length));
                _addToList(value);
            }
            
            copy.val('');
        };

        copy.bind('keydown', function(event) {
            if (event.which === 188 || event.which === 13) {
                _addTag(this);
                event.preventDefault();
                return false;
            }
        });
        
        if(input.val().length > 0) {
            var tags = input.val().split(/,[\s]*/);
            
            for (var i = 0; i < tags.length; i++) {
                _addToList(tags[i]);
            }
        }
    };
})(jQuery)
