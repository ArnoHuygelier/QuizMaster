const container = document.getElementById('question-container');
let questionIndex = parseInt(container.dataset.questionCount);

document.getElementById("add-question-btn").addEventListener("click", () => {
    addQuestion();
});

window.onload = () => {
    if (questionIndex === 0) {
        addQuestion();
    }
};

function addQuestion() {
    const container = document.getElementById('question-container');
    const card = document.createElement('div');
    card.className = 'card mb-4 pt-3 pb-3';
    card.setAttribute('data-index', questionIndex);

    const namePrefix = `Questions[${questionIndex}]`;

    card.innerHTML = `<input type="hidden" name="${namePrefix}.QuestionId" id="QuestionId_${questionIndex}" >
        <div class="d-flex justify-content-between align-items-center mb-2 pe-3">
            <h5>Question ${questionIndex + 1}</h5>
            <button type="button" class="btn btn-danger btn-sm" onclick="removeQuestion(this)">Remove</button>
        </div>

        <div class="form-group pe-3">
            <label class=form-label>Question Text</label>
            <input type="text" name="${namePrefix}.Text" class="form-control" placeholder="Enter your question">
            <span class="text-danger" data-valmsg-for="${namePrefix}.Text" data-valmsg-replace="true"></span>
        </div>

        <label class="mt-3 fw-bold d-block pe-3">Select the correct answer</label>

        ${[0, 1, 2, 3].map(i => `
            <div class="form-check mb-2 pe-3">
                <input class="form-check-input" type="radio" name="${namePrefix}.CorrectAnswerIndex" value="${i}" ${i === 0 ? 'checked' : ''} onchange="markCorrect(this)">
                <input type="hidden" name="${namePrefix}.Answers[${i}].Id">
                <input type="text" class="form-control" name="${namePrefix}.Answers[${i}].AnswerText" placeholder="Answer ${i + 1}">
                <input type="hidden" name="${namePrefix}.Answers[${i}].IsCorrect" value="${i === 0 ? 'true' : 'false'}">
                <span class="text-danger" data-valmsg-for="${namePrefix}.Answers[${i}].AnswerText" data-valmsg-replace="true"></span>
            </div>
        `).join('')}
    `;

    container.appendChild(card);
    questionIndex++;

    // Re-parse validation for newly added fields
    $.validator.unobtrusive.parse(card);
}

function removeQuestion(button) {
    const card = button.closest('.card');
    card.remove();
    renumberQuestions();
}

function renumberQuestions() {
    const container = document.getElementById('question-container');
    const cards = container.querySelectorAll('.card');
    questionIndex = 0;

    cards.forEach((card, newIndex) => {
        const oldIndex = card.dataset.index;
        card.setAttribute('data-index', newIndex);
        card.querySelector('h5').textContent = `Question ${newIndex + 1}`;

        const namePrefix = `Questions[${newIndex}]`;

        // Update all elements with name attributes
        const elements = card.querySelectorAll('[name]');
        elements.forEach(el => {
            const oldName = el.name;

            // Update question-level fields
            if (oldName.startsWith(`Questions[${oldIndex}]`)) {
                el.name = oldName.replace(`Questions[${oldIndex}]`, namePrefix);
            }

            // Update answer-level fields
            if (oldName.includes('.Answers[')) {
                const answerMatch = oldName.match(/\.Answers\[(\d+)\]/);
                if (answerMatch) {
                    const answerIndex = answerMatch[1];
                    el.name = el.name.replace(`Questions[${oldIndex}].Answers[${answerIndex}]`, `${namePrefix}.Answers[${answerIndex}]`);
                }
            }
        });

        // Update validation message attributes
        const validationSpans = card.querySelectorAll('[data-valmsg-for]');
        validationSpans.forEach(span => {
            const oldFor = span.getAttribute('data-valmsg-for');

            if (oldFor.startsWith(`Questions[${oldIndex}]`)) {
                span.setAttribute('data-valmsg-for', oldFor.replace(`Questions[${oldIndex}]`, namePrefix));
            }

            if (oldFor.includes('.Answers[')) {
                const answerMatch = oldFor.match(/\.Answers\[(\d+)\]/);
                if (answerMatch) {
                    const answerIndex = answerMatch[1];
                    span.setAttribute('data-valmsg-for', oldFor.replace(`Questions[${oldIndex}].Answers[${answerIndex}]`, `${namePrefix}.Answers[${answerIndex}]`));
                }
            }
        });

        // Update radio button values to match new answer indices
        const radios = card.querySelectorAll(`input[type="radio"][name="${namePrefix}.CorrectAnswerIndex"]`);
        radios.forEach((radio, idx) => {
            radio.value = idx;
        });

        questionIndex++;
    });

    // Re-parse the entire form for validation
    const form = $(container).closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
}

function markCorrect(radio) {
    const groupName = radio.name;
    const card = radio.closest('.card');
    const allOptions = card.querySelectorAll(`input[type="radio"][name="${groupName}"]`);

    allOptions.forEach((r, idx) => {
        const wrapper = r.closest('.form-check');
        wrapper.classList.remove('correct-selected');

        // Update the IsCorrect hidden field
        const answerPrefix = groupName.replace('CorrectAnswerIndex', `Answers[${idx}].IsCorrect`);
        const isCorrectInput = card.querySelector(`input[name="${answerPrefix}"]`);
        if (isCorrectInput) {
            isCorrectInput.value = (r === radio) ? 'true' : 'false';
        }
    });

    radio.closest('.form-check').classList.add('correct-selected');
}