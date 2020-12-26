const notify = {
    EType: {
        SUCCESS: 'success',
        DANGER: 'danger',
        WARNING: 'warning'
    },
    push: (_messenge, _type) => {
        let icon;
        switch (_type) {
            case notify.EType.SUCCESS:
                icon = 'fa fa-check';
                break;
            case notify.EType.DANGER:
                icon = 'fa fa-minus';
                break;
            default:
                icon = 'fa fa-check';
                break;
        }
        $.niftyNoty({
            type: _type,
            icon: icon,
            message: _messenge,
            container: 'floating',
            timer: 3000
        });
    },
    loading: () => {
        let html =
            `<div class="fade-loading" id='loading'>
                <div class="sk-double-bounce loading">
                  <div class="sk-child sk-double-bounce1"></div>
                  <div class="sk-child sk-double-bounce2"></div>
                </div>
             </div>`;
        $('body').append(html);
    },
    done: () => {
        $('#loading').remove();
    }
};