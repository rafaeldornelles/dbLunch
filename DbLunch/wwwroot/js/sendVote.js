function setSendVote() {
    $("#voting-form").submit(e => {
        e.preventDefault();
        const form = document.querySelector("#voting-form");
        const voteObj = {
            "voter_id": form.voter_id.value,
            "restaurant_id": form.restaurant_id.value,
            "date": form.voteDate.value
        }

        if (voteIsValid(voteObj)) {
            $.ajax({
                type: 'POST',
                url: '/Voting/RegisterVote',
                data: JSON.stringify(voteObj),
                contentType: "application/json",
                dataType: 'text',
                success: resp => window.location.reload(),
                error: (error) => showErrorModal(error.responseText, error.status)
            });
        }

    });
}

function voteIsValid(vote) {
    $(".invalid-feedback").hide();
    if (vote.restaurant_id == "") {
        $("#invalid-restaurant_id").show();
    }

    if (vote.voter_id == "") {
        $("#invalid-voter_id").show();
    }

    return vote.restaurant_id != "" && vote.voter_id != "";
}

function showErrorModal(message, status) {
    $("#modal-error-message").text(message ? `Mensagem: ${message}` : "Tente novamente mais tarde.");
    $("#modal-error-status").text(`Status: ${status}`);
    $("#errorModal").modal("show");
}