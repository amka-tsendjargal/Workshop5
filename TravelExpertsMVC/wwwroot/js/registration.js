
$(document).ready(function () {
    // Handle the change event of the Country dropdown and  Province/States dropdown
    $("#CustCountry").change(function () {
        var selectedCountry = $(this).val();
        updateStatesProvinces(selectedCountry);
        validatePostalCode(selectedCountry);
    });

    // Handle form submission to check password confirmation and phone number format    
    //$("form").submit(function () {
    //    // Clear previous error messages
    //    $("#passwordMismatchError").text("");
    //    $("#phoneFormatError").text("");

    //    var password = $("#UserPwd").val();
    //    var confirmPassword = $("#ConfirmPassword").val();
    //    if (password !== confirmPassword) {
    //        // Passwords don't match, show error message
    //        $("#passwordMismatchError").text("Password confirmation doesn't match.");
    //        return false; // Prevent form submission
    //    }

    //    // Phone number format validation using regex
    //    var phoneRegex = /^\d{3}-\d{3}-\d{4}$/;
    //    var homePhone = $("#CustHomePhone").val();
    //    var busPhone = $("#CustBusPhone").val();
    //    if (!phoneRegex.test(homePhone) || !phoneRegex.test(busPhone)) {
    //        // Phone number format is incorrect, show error message
    //        $("#phoneFormatError").text("Phone number format is incorrect.");
    //        return false; // Prevent form submission
    //    }

    //    // Allow form submission if both client-side and server-side validations pass
    //    return true;
    //});
});

function updateStatesProvinces(selectedCountry) {
    // Make an AJAX request to the controller action to get states/provinces
    $.ajax({
        url: "/Customer/GetStatesProvinces",
        type: "GET",
        data: { country: selectedCountry },
        success: function (data) {
            // Clear existing options in the State/Province dropdown
            $("#CustProv").empty();

            // Add new options based on the received data
            $.each(data, function (index, value) {
                $("#CustProv").append('<option value="' + value + '">' + value + '</option>');
            });
        },
        error: function () {
            alert("Failed to get states/provinces.");
        }
    });
}

function validatePostalCode(selectedCountry) {
    var postalCode = $("#CustPostal").val().trim();

// Check postal code format based on selected country
if (selectedCountry === "USA") {
        var regex = /^(\d{5})(-\d{4})?$/;
if (!regex.test(postalCode)) {
    // Invalid postal code format for USA
    $("#CustPostal").addClass("border-danger");
        } else {
    $("#CustPostal").removeClass("border-danger");
        }
    } else if (selectedCountry === "Canada") {
        var regex = /^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$/;
if (!regex.test(postalCode)) {
    // Invalid postal code format for Canada
    $("#CustPostal").addClass("border-danger");
        } else {
    $("#CustPostal").removeClass("border-danger");
        }
    } else {
    // For other countries, no specific validation is applied
    $("#CustPostal").removeClass("border-danger");
    }
}
