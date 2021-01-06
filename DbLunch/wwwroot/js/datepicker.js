let now;
let dateFormat;
function setDatePicker() {
    now = moment($("#voteDate").val(), "YYYY-MM-DD");
    dateFormat = 'DD, dd/mm/yyyy';

    $(".datepicker").datepicker({
        autoclose: true,
        daysOfWeekDisabled: [0, 6],
        format: dateFormat,
        weekStart: 1,
        language: "pt-BR"
    });
    $(".datepicker").datepicker("setDate", now.toDate())
        .on("changeDate", e => {
            const dateSelected = $(".datepicker").first().datepicker("getDate");
            redirectToDate(moment(dateSelected));
        });

    $("#prev").click(prevDay);
    $("#next").click(nextDay);

}
function nextDay() {
    do {
        now = now.add(1, "days");
    } while ([0, 6].includes(now.weekday()));

    $(".datepicker").datepicker("setDate", now.toDate());
    redirectToDate(now);
};

function prevDay() {
    do {
        now = now.subtract(1, "days");
    } while ([0, 6].includes(now.weekday()));
    $(".datepicker").datepicker("update", now.toDate());
    redirectToDate(now);
}

function redirectToDate(date) {
    window.location.search = `date=${date.format("YYYY-MM-DD")}`;
}
