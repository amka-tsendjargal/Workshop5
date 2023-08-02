
$(document).ready(function () {
    var options = {
        "": [],
        "Canada": ["AB", // Alberta
            "BC", // British Columbia
            "MB", // Manitoba
            "NB", // New Brunswick
            "NL", // Newfoundland and Labrador
            "NS", // Nova Scotia
            "ON", // Ontario
            "PE", // Prince Edward Island
            "QC", // Quebec
            "SK"  // Saskatchewan
        ],
        "USA": [
            "AL", // Alabama
            "AK", // Alaska
            "AZ", // Arizona
            "AR", // Arkansas
            "CA", // California
            "CO", // Colorado
            "CT", // Connecticut
            "DE", // Delaware
            "FL", // Florida
            "GA", // Georgia
            "HI", // Hawaii
            "ID", // Idaho
            "IL", // Illinois
            "IN", // Indiana
            "IA", // Iowa
            "KS", // Kansas
            "KY", // Kentucky
            "LA", // Louisiana
            "ME", // Maine
            "MD", // Maryland
            "MA", // Massachusetts
            "MI", // Michigan
            "MN", // Minnesota
            "MS", // Mississippi
            "MO", // Missouri
            "MT", // Montana
            "NE", // Nebraska
            "NV", // Nevada
            "NH", // New Hampshire
            "NJ", // New Jersey
            "NM", // New Mexico
            "NY", // New York
            "NC", // North Carolina
            "ND", // North Dakota
            "OH", // Ohio
            "OK", // Oklahoma
            "OR", // Oregon
            "PA", // Pennsylvania
            "RI", // Rhode Island
            "SC", // South Carolina
            "SD", // South Dakota
            "TN", // Tennessee
            "TX", // Texas
            "UT", // Utah
            "VT", // Vermont
            "VA", // Virginia
            "WA", // Washington
            "WV", // West Virginia
            "WI", // Wisconsin
            "WY"  // Wyoming
        ]
    };

    
    function populateStateProvinceList(selectedCountry) {
        var stateProvinceList = $("#CustProv");
        stateProvinceList.empty();

        options[selectedCountry].forEach(function (option) {
            stateProvinceList.append($("<option>").text(option).val(option));
        });
    }
   
    var defaultSelection = $("#CustCountry").val();
    populateStateProvinceList(defaultSelection);

    $("#CustCountry").on("change", function () {
        var selectedCountry = $(this).val();
        populateStateProvinceList(selectedCountry);
        validatePostalCode(selectedCountry);
    })

    $("#CustPostal").on("change", function () {
        var selectedCountry = $("#CustCountry").val();
        if (selectedCountry !== "") {
            validatePostalCode(selectedCountry);
        }
    })


    function formatPhoneNumber(inputField) {
        // Get the value of the input field that triggered the event
        let phoneNumber = inputField.val().replace(/\D/g, ""); // Remove all non-digit characters

        // Check if the input is not empty and has a valid length
        if (phoneNumber.length > 0 && phoneNumber.length <= 10) {
            phoneNumber = phoneNumber.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3"); // Apply the format XXX-XXX-XXXX
        }

        // Set the formatted phone number back to the input field
        inputField.val(phoneNumber);
    }

    $("#CustHomePhone").on("input", function () {
        formatPhoneNumber($(this)); // Pass the input field as an argument to the formatPhoneNumber function
    });

    $("#CustBusPhone").on("input", function () {
        formatPhoneNumber($(this)); // Pass the input field as an argument to the formatPhoneNumber function
    });




})