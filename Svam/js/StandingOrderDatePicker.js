function unavailableM(date) {
        if (date.getDay() === 1) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }
    function unavailableT(date) {
        if (date.getDay() === 2) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }
    function unavailableW(date) {
        if (date.getDay() === 3) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }
    function unavailableTh(date) {
        if (date.getDay() ===4) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }
    function unavailableF(date) {
        if (date.getDay() ===5) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }
    function unavailableSat(date) {
        if (date.getDay() ===6) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }
    function unavailableSun(date) {
        if (date.getDay() ===0) {
            return [true, ""];
        } else {
            return [false, "", "Unavailable"];
        }
    }

    jQuery(document).ready(function () {
        /* Monday DatePicker*/
        jQuery(".MrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableM
        });
        /* Tuesday DatePicker*/
        jQuery(".TrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableT
        });
        /* Wednesday DatePicker*/
        jQuery(".WrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableW
        });
        /* Thuresday DatePicker*/
        jQuery(".ThrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableTh
        });
        /* Friday DatePicker*/
        jQuery(".FrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableF
        });
        /* Saturday DatePicker*/
        jQuery(".SatrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableSat
        });
        /* Sunday DatePicker*/
        jQuery(".SunrepDate").datepicker({
            dateFormat: "mm/dd/yy",
            minDate: 1,
            beforeShowDay: unavailableSun
        });
    });