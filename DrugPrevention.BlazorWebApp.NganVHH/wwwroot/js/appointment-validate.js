// Appointment validation script for Blazor

// Validate form cuộc hẹn cho Blazor: chỉ validate các trường đang enable (không bị disabled)
function validateAppointmentForm() {
    var isValid = true;

    // Validate UserID
    var userInput = document.querySelector('select[name*="UserID"]') || 
                   document.querySelector('select[id*="UserID"]') ||
                   document.querySelectorAll('select')[0];
    if (userInput && !userInput.disabled && (!userInput.value || userInput.value === '')) {
        isValid = false;
        showFieldError(userInput, 'Bắt buộc chọn người dùng!');
    } else if (userInput && !userInput.disabled) {
        clearFieldError(userInput);
    }

    // Validate ConsultantID
    var consultantInput = document.querySelector('select[name*="ConsultantID"]') || 
                         document.querySelector('select[id*="ConsultantID"]') ||
                         document.querySelectorAll('select')[1];
    if (consultantInput && !consultantInput.disabled && (!consultantInput.value || consultantInput.value === '')) {
        isValid = false;
        showFieldError(consultantInput, 'Bắt buộc chọn chuyên gia!');
    } else if (consultantInput && !consultantInput.disabled) {
        clearFieldError(consultantInput);
    }

    // Validate AppointmentDateTime
    var dateInput = document.querySelector('input[type="datetime-local"]') || 
                   document.querySelector('input[name*="AppointmentDateTime"]') ||
                   document.querySelector('input[id*="AppointmentDateTime"]');
    if (dateInput && !dateInput.disabled) {
        if (!dateInput.value) {
            isValid = false;
            showFieldError(dateInput, 'Bắt buộc chọn ngày giờ!');
        } else {
            // Parse local datetime
            var parts = dateInput.value.split(/[-T:]/);
            var inputDate = new Date(parts[0], parts[1] - 1, parts[2], parts[3], parts[4]);
            var now = new Date();
            if (inputDate < now) {
                isValid = false;
                showFieldError(dateInput, 'Ngày giờ phải là hiện tại hoặc tương lai!');
            } else {
                clearFieldError(dateInput);
            }
        }
    }

    // Validate Duration
    var durationInput = document.querySelector('select[name*="Duration"]') || 
                       document.querySelector('select[id*="Duration"]');
    if (durationInput && !durationInput.disabled) {
        var val = parseInt(durationInput.value);
        if (!durationInput.value || isNaN(val) || val < 30 || val > 480) {
            isValid = false;
            showFieldError(durationInput, 'Thời lượng từ 30 đến 480 phút!');
        } else {
            clearFieldError(durationInput);
        }
    }

    // Validate ConsultationType
    var consultTypeInput = document.querySelector('select[name*="ConsultationType"]') || 
                          document.querySelector('select[id*="ConsultationType"]');
    if (consultTypeInput && !consultTypeInput.disabled && (!consultTypeInput.value || consultTypeInput.value === '')) {
        isValid = false;
        showFieldError(consultTypeInput, 'Bắt buộc chọn hình thức tư vấn!');
    } else if (consultTypeInput && !consultTypeInput.disabled) {
        clearFieldError(consultTypeInput);
    }

    // Validate AssessmentType (bắt buộc)
    var assessmentInput = document.querySelector('input[name*="AssessmentType"]') || 
                         document.querySelector('input[id*="AssessmentType"]');
    if (assessmentInput && !assessmentInput.disabled && (!assessmentInput.value || assessmentInput.value.trim() === '')) {
        isValid = false;
        showFieldError(assessmentInput, 'Bắt buộc nhập loại đánh giá!');
    } else if (assessmentInput && !assessmentInput.disabled) {
        clearFieldError(assessmentInput);
    }

    // Validate FeedbackRating (không bắt buộc, nhưng nếu có thì phải là số nguyên từ 1-5)
    var feedbackInput = document.querySelector('input[name*="FeedbackRating"]') || 
                       document.querySelector('input[id*="FeedbackRating"]') ||
                       document.querySelector('input[type="number"]');
    if (feedbackInput && !feedbackInput.disabled && feedbackInput.value && feedbackInput.value.trim() !== '') {
        var feedbackValue = feedbackInput.value.trim();
        
        // Kiểm tra có phải số không
        if (isNaN(feedbackValue)) {
            isValid = false;
            showFieldError(feedbackInput, 'Đánh giá phải là số!');
        }
        // Kiểm tra có phải số thập phân không
        else if (feedbackValue.includes('.') || feedbackValue.includes(',')) {
            isValid = false;
            showFieldError(feedbackInput, 'Đánh giá phải là số nguyên (1, 2, 3, 4, hoặc 5), không được là số thập phân!');
        }
        // Kiểm tra range từ 1-5
        else {
            var feedbackNum = parseInt(feedbackValue);
            if (feedbackNum < 1 || feedbackNum > 5) {
                isValid = false;
                showFieldError(feedbackInput, 'Đánh giá phải từ 1 đến 5 sao!');
            } else {
                clearFieldError(feedbackInput);
            }
        }
    } else if (feedbackInput && !feedbackInput.disabled) {
        clearFieldError(feedbackInput);
    }

    // Validate Notes (không bắt buộc, nhưng nếu có thì kiểm tra độ dài)
    var notesInput = document.querySelector('textarea[name*="Notes"]') || 
                    document.querySelector('textarea[id*="Notes"]') ||
                    document.querySelector('form textarea');
    if (notesInput && !notesInput.disabled && notesInput.value && notesInput.value.length > 1000) {
        isValid = false;
        showFieldError(notesInput, 'Ghi chú tối đa 1000 ký tự!');
    } else if (notesInput && !notesInput.disabled) {
        clearFieldError(notesInput);
    }

    return isValid;
}

// Show error message for a field
function showFieldError(input, message) {
    input.classList.add('is-invalid');
    
    let errorSpan = input.parentNode.querySelector('.invalid-feedback');
    if (!errorSpan) {
        errorSpan = document.createElement('div');
        errorSpan.className = 'invalid-feedback';
        input.parentNode.appendChild(errorSpan);
    }
    
    errorSpan.innerHTML = '<i class="fas fa-exclamation-circle me-1"></i>' + message;
    errorSpan.style.display = 'block';
}

// Clear error message for a field
function clearFieldError(input) {
    input.classList.remove('is-invalid');
    
    let errorSpan = input.parentNode.querySelector('.invalid-feedback');
    if (errorSpan) {
        errorSpan.style.display = 'none';
        errorSpan.innerHTML = '';
    }
}

// Clear all error messages
function clearAllErrors() {
    document.querySelectorAll('.invalid-feedback').forEach(function(errorSpan) {
        errorSpan.style.display = 'none';
        errorSpan.innerHTML = '';
    });
    document.querySelectorAll('.is-invalid').forEach(function(input) {
        input.classList.remove('is-invalid');
    });
}

// Setup validation for Blazor - clear errors on input change
function setupInputValidation() {
    document.querySelectorAll('input, select, textarea').forEach(function(input) {
        input.addEventListener('change', function() {
            if (this.classList.contains('is-invalid')) {
                clearFieldError(this);
            }
        });
        
        input.addEventListener('input', function() {
            if (this.classList.contains('is-invalid')) {
                clearFieldError(this);
            }
        });
    });
}

// Setup event listeners cho input validation
document.addEventListener('DOMContentLoaded', setupInputValidation);
window.addEventListener('load', setupInputValidation);

// Delayed setup for Blazor components
setTimeout(setupInputValidation, 1000);
setTimeout(setupInputValidation, 2000);

// Make functions globally available for Blazor to call
window.validateAppointmentForm = validateAppointmentForm;
window.clearAllErrors = clearAllErrors; 