
$(document).ready(function () {
    $("#company").change(function () {
        var companyId = $(this).val();
        if (companyId) {
            $.ajax({
                url: "/Ajax/LoadBranchToCompanyId/" + companyId,
                type: "POST",
                dataType: "json",
                success: function (res) {
                   
                    $("#BranchId").html("");
                    $("#CompanyToDepartment_DepartmentId").html("");
                    var options = "";
                    for (var data of res.branches) {
                        options += `<option value="${data.id}">${data.name}</option>`;
                    }
                    for (var i = 0; i < res.branches.length; i++) {
                    }

                    $("#BranchId").append(options);


                    $("#CompanyToDepartment_DepartmentId").html("");
                    for (var department of res.departments) {

                        var option = `<option value="${department.departmentId}">${department.name}</option>`;
                        $("#CompanyToDepartment_DepartmentId").append(option);
                    }
                    //document.getElementById('CompanyToDepartment_DepartmentId').dispatchEvent(new Event("change"));
                }
            });
        }
    })
    $("#CompanyToDepartment_DepartmentId").change(function () {
        var department = $(this).val();
        if (department) {
            $.ajax({
                url: "/Ajax/LoadBranchToDepartmentId/" + department,
                type: "POST",
                dataType: "json",
                success: function (res) {
                    var options = "";
                    $("#PositionId").html("");
                    for (var data of res.position) {
                        options += `<option value="${data.id}">${data.name}</option>`;
                    }
                    
                    $("#PositionId").append(options);
                }
            })
          }
    })


    $(".Search_Select_Penal").change(function () {
        var SelectOption = $(this).val();
        console.log(SelectOption);
        $.ajax({
            url: "/Penals/PartiallPenal/",
            data: { months: SelectOption },
            type: "POST",
            dataType: "html",
            success: function (res) {
                $(".tBody").children().remove();
                $(".tBody").append(res);
            }
        })
    })

    //////////////////////SearchMontsh///////////////////////////////
    $(".Search_Select").change(function () {
        var SelectOption = $(this).val();
        $.ajax({
            url: "/Bonuses/PatrialIndex/",
            data: { months: SelectOption },
            type: "POST",
            dataType: "html",
            success: function (res) {
                $(".tBody").children().remove();
                $(".tBody").append(res);
            }
        })
    });


   

    //////////////////////SearchMontsh///////////////////////////////



});



//Search Name for employee


function Contains(text_one, text_two) {
    if (text_one.indexOf(text_two) !== -1) {
        return true;
    }
}

$("#searchValue").keyup(function () {
    var searchText = $("#searchValue").val().toLowerCase();
    $(".emp").each(function () {
        if (!Contains($(this).text().toLowerCase(), searchText)) {
            $(this).hide();
        } else {
            $(this).show();
        }
    });
});



