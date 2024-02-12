// JavaScript source code

const baseUrl = window.location.origin;
$(document).ready(function () {
    $("#vehicleTable").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#vehicleTable_wrapper .col-md-6:eq(0)');

    $("#brandTable").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#brandTable_wrapper .col-md-6:eq(0)');

    $("#mileageTable").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#mileageTable_wrapper .col-md-6:eq(0)');
    
    $("#serviceTable").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#serviceTable_wrapper .col-md-6:eq(0)');
    $("#partTable").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#partTable_wrapper .col-md-6:eq(0)'); 
    $("#customerTable").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#customerTable_wrapper .col-md-6:eq(0)');
    $("#serviceContractTable").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "order": [[1, "desc"]],
        "buttons": ["copy", "csv", "excel", "pdf", "print"]
    }).buttons().container().appendTo('#vehicleTable_wrapper .col-md-6:eq(0)');


    $('.partCheckbox').change(function () {
        calculateTotalCost();
        const discountPercentage = parseFloat($("#DiscountPartCost").val());
        const costBeforeDiscount = parseFloat($("#totalPartCost").val());
        // Calculate discounted cost
        const discountedCost = costBeforeDiscount * (1 - discountPercentage / 100);
        // Update the cost after discount field
        $('#totalPartCostAfterDiscount').val(discountedCost.toFixed(3));
        $('.totalPartsAmount').html(discountedCost.toFixed(3)+" KD");
        updateTotalContractCost();
    });

    

    var allCosts = $('#allCosts').val();
    var linkElement = $('#proceedButton');

    var currentUrl = linkElement.attr('href');
    var updatedUrl = currentUrl + "&TotCost=" + allCosts;

    linkElement.attr('href', updatedUrl);

    $('.quantityInput').on('change', function () {
        updateRowTotalCost($(this).closest('tr'));
        calculateTotalCost();
        $('.totalPartsAmount').html(totalPartCostAfterDiscount.toFixed(3) + " KD");
        updateTotalContractCost();
    });

    

    

    //**** Populate the Job Title dropdown list based on selected Theme (Category)*/
    $("#SC_customerId").change(function () {
        var selectedCustomerId = $(this).val();
        // Enable textfields before sending AJAX request
        $("#SC_CustomerName").prop('disabled', false);
        $("#SC_CivilId").prop('disabled', false);
        $("#SC_Phone").prop('disabled', false);
        $("#SC_Email").prop('disabled', false);
        $("#SC_Address").prop('disabled', false);

        $.ajax({
            url: '/Customer/GetCustomerDetails',
            type: 'GET',
            data: { customerId: selectedCustomerId },
            success: function (data) {
                $("#SC_CustomerName").val(data.customerName);
                $("#SC_CivilId").val(data.civilId);
                $("#SC_Phone").val(data.phone);
                $("#SC_Email").val(data.email);
                $("#SC_Address").val(data.address);

                // Disable textfields after successful AJAX request
                $("#SC_CustomerName").prop('disabled', true);
                $("#SC_CivilId").prop('disabled', true);
                $("#SC_Phone").prop('disabled', true);
                $("#SC_Email").prop('disabled', true);
                $("#SC_Address").prop('disabled', true);
            }
        });
    });

    $('#ServiceContractSubmit').click(function () {
        // Number of rows of the Parts Table 
        var ServicePartTableRows = getTableRowCount("#SC_selected_parts");
        $('#SC_nbParts').val(ServicePartTableRows);
        calculateTotalCost();
    });

    $('.partCheckbox').on('change', function () {
        var isChecked = $(this).prop('checked');
        var row = $(this).closest('tr');
        var checkedStatusInput = row.find('.checkedStatus');

        if (isChecked) {
            // Checkbox is checked
            checkedStatusInput.val('checked');
        } else {
            // Checkbox is unchecked
            checkedStatusInput.val(''); 
        }
    });

    $('#SC_manually_parts').on('click', '.add-new-part-row', function () {
        var newRow = '<tr>';
        newRow += '<td width="17%"><input type="text" class="form-control" style="width: 100%; box - sizing: border - box; " name="ManualPartNumber[]" value="" required /></td>';
        newRow += '<td width="43%"><input type="text" class="form-control" style="width: 100%; box-sizing: border-box;" name="ManualPartName[]" value="" required /></td>';
        newRow += '<td width="10%"><input type="number"  style="width: 100%; box-sizing: border-box;" name="ManualUnitCost[]" class="form-control priceInputManual" min="0" step="0.001" value="" required /></td>';
        newRow += '<td width="7%"><input type="number"  style="width: 100%; box-sizing: border-box;" name="ManualPartQty[]" class="form-control quantityInputManual" value="1" min="1" step="1" required /></td>';
        newRow += '<td width="10%" ><input type="text" class="form-control totalCostManual" value="0.00 KD" readonly /> </td>';
        newRow += '<td><button type="button" class="btn btn-sm btn-outline-primary add-new-part-row" > <i class="fa fa-plus"></i></button>';
        newRow += '<button type="button" class="btn btn-sm btn-outline-danger remove-part-row ml-1" > <i class="fa fa-trash"></i></button>';
        newRow += '</td > ';
        newRow += '</tr>';
        $('#SC_manually_parts tbody').append(newRow);
    });
    $('#manualParts').on('click', function () {
        var newRow = '<tr>';
        newRow += '<td width="17%"><input type="text" class="form-control" style="width: 100%; box - sizing: border - box; " name="ManualPartNumber[]" value="" required /></td>';
        newRow += '<td width="43%"><input type="text" class="form-control" style="width: 100%; box-sizing: border-box;" name="ManualPartName[]" value="" required /></td>';
        newRow += '<td width="10%"><input type="number"  style="width: 100%; box-sizing: border-box;" name="ManualUnitCost[]" class="form-control priceInputManual" min="0" step="0.001" value="" required /></td>';
        newRow += '<td width="7%"><input type="number"  style="width: 100%; box-sizing: border-box;" name="ManualPartQty[]" class="form-control quantityInputManual" value="1" min="1" step="1" required /></td>';
        newRow += '<td width="10%" ><input type="text" class="form-control totalCostManual" value="0.00 KD" readonly /> </td>';
        newRow += '<td><button type="button" class="btn btn-sm btn-outline-primary add-new-part-row" > <i class="fa fa-plus"></i></button>';
        newRow += '<button type="button" class="btn btn-sm btn-outline-danger remove-part-row ml-1" > <i class="fa fa-trash"></i></button>';
        newRow += '</td > ';
        newRow += '</tr>';
        $('#SC_manually_parts tbody').append(newRow);
    });

    ////If we change the Vehicle Type
    //$('#changingVehicleType').on('change', function () {
    //    var typeId = $('#changingVehicleType').val();
    //    var vehicleId = $('#EditContractVehicleId').val();
    //    //alert(typeId);
    //    //alert(vehicleId);

    //    $.ajax({
    //        url: '/ServiceContract/GetVehicleParts',
    //        type: 'GET',
    //        data: {
    //            typeId: typeId,
    //            vehicleId: vehicleId
    //        },
    //        success: function (data) {
    //            //alert(data);
    //            $('#SC_selected_parts tbody').html(data);
    //            $('#showCode').text(data);

    //            ////Update the total Cost 
    //            //var totalCost = 0;
    //            //$('#ServiceContractMileagesTable tbody tr').each(function () {
    //            //    totalCost += parseFloat($(this).find('td:eq(1)').text().replace(' KD', ''));
    //            //});
    //            //// Display total cost 
    //            //$('#totalServiceCostBeforeDiscount').val(totalCost.toFixed(3));
    //            ////Get the discount 
    //            //var discount = $('#DiscountServiceCost').val();
    //            //var TotalCostAfterDiscount = totalCost * (1 - (parseFloat(discount) / 100));
    //            //// Display total cost after discount
    //            //$('#totalServiceCost').val(TotalCostAfterDiscount.toFixed(3));
    //            //$('.totalServiceAmount').html(TotalCostAfterDiscount.toFixed(3) + " KD");
    //            //updateTotalContractCost();
    //        },
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            alert("Error fetching services:", errorThrown);

    //        }
    //    });
    //});




    
    $('#changingStartingMileage').on('change', function () {
        var startingMileageId = $('#changingStartingMileage').val();
        var endingMileage = $('#changingEndingMileage').val();
        var vehicleId = $('#EditContractVehicleId').val();
        var startingMileageText = $("#changingStartingMileage option:selected").text();
        var endingMileageText = $("#changingEndingMileage option:selected").text();
        // Validate selections (ensure starting mileage is less than ending mileage)
        if (!startingMileageId || !endingMileage || parseInt(startingMileageId) >= parseInt(endingMileage)) {
            alert("Please select a starting mileage less than the ending mileage.");
            return; 
        }
        
        $.ajax({
            url: '/ServiceContract/CalculateMileageInterval',
            type: 'GET',
            data: {
                startMileage: startingMileageText,
                endMileage: endingMileageText
            },
            success: function (data) {
                $('#SC_ContractDuration').val(data);
                calculateExpiryDate();
                // execute the other code 
                $.ajax({
                    url: '/ServiceContract/GetMileageServices',
                    type: 'GET',
                    data: {
                        vehicleId: vehicleId,
                        startMileageId: startingMileageId,
                        endMileageId: endingMileage
                    },
                    success: function (data) {
                        $('#ServiceContractMileagesTable tbody').html(data);

                        //Update the total Cost 
                        var totalCost = 0;
                        $('#ServiceContractMileagesTable tbody tr').each(function () {
                            totalCost += parseFloat($(this).find('td:eq(1)').text().replace(' KD', ''));
                        });
                        // Display total cost 
                        $('#totalServiceCostBeforeDiscount').val(totalCost.toFixed(3));
                        //Get the discount 
                        var discount = $('#DiscountServiceCost').val();
                        var TotalCostAfterDiscount = totalCost * (1 - (parseFloat(discount) / 100));
                        // Display total cost after discount
                        $('#totalServiceCost').val(TotalCostAfterDiscount.toFixed(3));
                        $('.totalServiceAmount').html(TotalCostAfterDiscount.toFixed(3) + " KD");
                        updateTotalContractCost();
                        $('#StartMileage').val(startingMileageText);
                        $('#StartMileageCaption').val(startingMileageText);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert("Error fetching services:", errorThrown);

                    }
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error calculating mileage interval:", errorThrown);
            }

        });
    });

    $('#changingEndingMileage').on('change', function () {
        var startingMileageId = $('#changingStartingMileage').val();
        var endingMileage = $('#changingEndingMileage').val();
        var vehicleId = $('#EditContractVehicleId').val();
        var startingMileageText = $("#changingStartingMileage option:selected").text();
        var endingMileageText = $("#changingEndingMileage option:selected").text();
        // Validate selections (ensure starting mileage is less than ending mileage)
        if (!startingMileageId || !endingMileage || parseInt(startingMileageId) >= parseInt(endingMileage)) {
            alert("Please select a starting mileage less than the ending mileage.");
            return;
        }
        $.ajax({
            url: '/ServiceContract/CalculateMileageInterval',
            type: 'GET',
            data: {
                startMileage: startingMileageText,
                endMileage: endingMileageText
            },
            success: function (data) {
                $('#SC_ContractDuration').val(data);
                calculateExpiryDate();
                // execute the other code 
                $.ajax({
                    url: '/ServiceContract/GetMileageServices',
                    type: 'GET',
                    data: {
                        vehicleId: vehicleId,
                        startMileageId: startingMileageId,
                        endMileageId: endingMileage
                    },
                    success: function (data) {
                        $('#ServiceContractMileagesTable tbody').html(data);

                        //Update the total Cost 
                        var totalCost = 0;
                        $('#ServiceContractMileagesTable tbody tr').each(function () {
                            totalCost += parseFloat($(this).find('td:eq(1)').text().replace(' KD', ''));
                        });
                        // Display total cost 
                        $('#totalServiceCostBeforeDiscount').val(totalCost.toFixed(3));
                        //Get the discount 
                        var discount = $('#DiscountServiceCost').val();
                        var TotalCostAfterDiscount = totalCost * (1 - (parseFloat(discount) / 100));
                        // Display total cost after discount
                        $('#totalServiceCost').val(TotalCostAfterDiscount.toFixed(3));
                        $('.totalServiceAmount').html(TotalCostAfterDiscount.toFixed(3) + " KD");
                        updateTotalContractCost();
                        $('#EndMileage').val(endingMileageText);
                        $('#EndMileageCaption').val(endingMileageText);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert("Error fetching services:", errorThrown);

                    }
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error calculating mileage interval:", errorThrown);
            }

        });
    });

    function calculateExpiryDate() {
        var startDate = new Date($('#SC_ContractStartingDate').val());
        var contractDurationYears = parseInt($('#SC_ContractDuration').val());

        // Check if the start date is valid
        if (!isNaN(startDate.getTime()) && contractDurationYears > 0) {
            // Calculate the expiry date by adding the contract duration years to the start date
            var expiryDate = new Date(startDate.getFullYear() + contractDurationYears, startDate.getMonth(), startDate.getDate());

            // Format the expiry date as "YYYY-MM-DD" and set it to the ExpiryDate input field
            var formattedExpiryDate = expiryDate.toISOString().split('T')[0];
            $('#SC_ContractExpiryDate').val(formattedExpiryDate);
        } else {
            // Handle invalid start date or contract duration
            alert("Invalid start date or contract duration.");
        }
    }

    



    $('#SC_manually_parts').on('click', '.remove-part-row', function () {
        if (confirm('Are you sure you want to remove this part?')) {
            $(this).closest('tr').remove();
            updateTotalContractCost();
        }
    });

    $('#SC_manually_parts').on('change', '.quantityInputManual, .priceInputManual', function () {
        var $row = $(this).closest('tr');
        var quantity = parseFloat($row.find('.quantityInputManual').val());
        var price = parseFloat($row.find('.priceInputManual').val());
        var totalCost = quantity * price;

        // Update the TotalCost cell
        $row.find('.totalCostManual').val(totalCost.toFixed(2) + " KD"); // Format to 2 decimal places

        // Calculate and update the total price before discount
        var totalManualCostBeforeDiscount = 0;
        $('#SC_manually_parts tbody tr').each(function () {
            var rowTotalCost = parseFloat($(this).find('.totalCostManual').val().replace(" KD", ""));
            totalManualCostBeforeDiscount += rowTotalCost;
        });

        // Apply discount and calculate total price after discount
        var discountPercentage = parseFloat($('#DiscountManualCost').val());
        var totalManualCostAfterDiscount = totalManualCostBeforeDiscount * (1 - discountPercentage / 100);

        $('#totalManualCostBeforeDiscount').val(totalManualCostBeforeDiscount.toFixed(3));
        $('#totalManualCost').val(totalManualCostAfterDiscount.toFixed(3));
        $('.totalAdditionalAmount').html(totalManualCostAfterDiscount.toFixed(3) + " KD");
        updateTotalContractCost();
        


    });

   


    // changing Service Cost after discount 
    $('#DiscountServiceCost').on('change', function () {
        const totalServiceCostBeforeDiscount = document.getElementById("totalServiceCostBeforeDiscount");
        const totalServiceCostAfterDiscount = document.getElementById("totalServiceCost");
        const discountPercentage = parseFloat($(this).val());
        const costBeforeDiscount = parseFloat(totalServiceCostBeforeDiscount.value);
        // Calculate discounted cost
        const discountedCost = costBeforeDiscount * (1 - discountPercentage / 100);
        // Update the cost after discount field
        totalServiceCostAfterDiscount.value = discountedCost.toFixed(2);
        $('.totalServiceAmount').html(discountedCost.toFixed(3) + " KD");
        updateTotalContractCost();

    });

    // changing Parts Cost after discount 
    $('#DiscountPartCost').on('change', function () {
        const totalPartCostBeforeDiscount = document.getElementById("totalPartCost");
        const totalPartCostAfterDiscount = document.getElementById("totalPartCostAfterDiscount");
        const discountPercentage = parseFloat($(this).val());
        const costBeforeDiscount = parseFloat(totalPartCostBeforeDiscount.value);
        // Calculate discounted cost
        const discountedCost = costBeforeDiscount * (1 - discountPercentage / 100);
        // Update the cost after discount field
        totalPartCostAfterDiscount.value = discountedCost.toFixed(2);
        $('.totalPartsAmount').html(discountedCost.toFixed(3) + " KD");
        updateTotalContractCost();

    });

    // changing Manual Parts Cost after discount 
    $('#DiscountManualCost').on('change', function () {
        const totalManualCostBeforeDiscount = document.getElementById("totalManualCostBeforeDiscount");
        const totalManualCostAfterDiscount = document.getElementById("totalManualCost");
        const discountPercentage = parseFloat($(this).val());
        const costBeforeDiscount = parseFloat(totalManualCostBeforeDiscount.value);
        // Calculate discounted cost
        const discountedCost = costBeforeDiscount * (1 - discountPercentage / 100);
        // Update the cost after discount field
        totalManualCostAfterDiscount.value = discountedCost.toFixed(2);
        $('.totalAdditionalAmount').html(discountedCost.toFixed(2) + " KD");
        updateTotalContractCost();

    });

    
    // *******************
    // *******************
    // **** FUNCTIONS ****
    // *******************
    // *******************

    //Calculate the TOTAL Contract Cost
    function updateTotalContractCost() {
        var serviceCost = parseFloat($('#totalServiceCost').val()) || 0;
        var partCost = parseFloat($('#totalPartCostAfterDiscount').val()) || 0;
        var additionalCost = parseFloat($('#totalManualCost').val()) || 0;
        var totalCost = serviceCost + partCost + additionalCost;

        $('.totalAmount').text(totalCost.toFixed(3) + ' KD');
        $('#FinalAmount').val(totalCost.toFixed(3));
    }

    function updateRowTotalCost(row) {
        var quantity = parseFloat(row.find('.quantityInput').val()) || 0;
        var unitPrice = parseFloat(row.find('.partCheckbox').data('unit-price')) || 0;
        var totalCost = quantity * unitPrice;

        row.find('.totalCost').text(totalCost.toFixed(3) + ' KD');
    }

   

    function calculateTotalCost() {
        var totalCost = 0;
        var allCosts = 0;
        var serviceCost = parseFloat($('#totalServiceCost').val());

        // Iterate over each checked checkbox
        $('.partCheckbox:checked').each(function () {
            var row = $(this).closest('tr');

            updateRowTotalCost(row);

            // Get the total cost for the current row
            var partTotalCost = parseFloat(row.find('.totalCost').text()) || 0;
            
            // Add the total cost of the current row to the overall total
            totalCost += partTotalCost;
            allCosts += partTotalCost;
        });

        allCosts += serviceCost;

        $('#totalPartCost').val(totalCost.toFixed(3));
        var discount = $('#DiscountPartCost').val();
        var partCostAfterDiscount = totalCost * (1 - (parseFloat(discount) / 100));
        $('#totalPartCostAfterDiscount').val(partCostAfterDiscount.toFixed(3));
        $('totalPartsAmount').html(partCostAfterDiscount.toFixed(3)+" KD");
        //$('#allCosts').val(allCosts.toFixed(3));
    }


    $('.select-btn').on('click', function () {
        // Get the clicked row
        var row = $(this).closest('tr');

        // Retrieve data from the row cells
        var custCode = row.find('td:eq(0)').text();
        var custName = row.find('td:eq(1)').text();
        var model = row.find('td:eq(2)').text();
        var purshYear = row.find('td:eq(3)').text();
        var regDate = row.find('td:eq(4)').text();
        var vin = row.find('td:eq(5)').text();
        var licence = row.find('td:eq(6)').text();
        var customerTel = row.find('td:eq(7)').text();
        var email = row.find('td:eq(8)').text();

        // Display data in input fields
        $('#custCodeInput').val(custCode);
        $('#custNameInput').val(custName);
        $('#modelInput').val(model);
        $('#purshYearInput').val(purshYear);
        $('#regDateInput').val(regDate);
        $('#vinInput').val(vin);
        $('#licenceInput').val(licence);
        $('#customerTelInput').val(customerTel);
        $('#emailInput').val(email);
    });

    //Warning when attemping to delete
    function confirmDelete() {
        return confirm("Are you sure you want to delete this item?");
    }

    function getTableRowCount(tableId) {
        //tableId should include the '#' caracter 
        var numRows = $(tableIdText).find('tbody tr').length;
        return numRows;
    }

    


    
});
