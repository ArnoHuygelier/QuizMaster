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

    card.innerHTML = `<input type="hidden" name="Id" id="Id" >
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
                    <input class="form-check-input" type="radio" name="${namePrefix}.Correct" value="${i}" onchange="markCorrect(this)">
                    <input type="text" class="form-control" name="${namePrefix}.Answers[${i}].AnswerText" placeholder="Answer ${i + 1}">
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

    cards.forEach((card, i) => {
        card.setAttribute('data-index', i);
        card.querySelector('h5').textContent = `Question ${i + 1}`;
        const namePrefix = `Questions[${i}]`;

        // Rename question text input
        const questionInput = card.querySelector('input[type="text"][name*=".Text"]');
        questionInput.name = `${namePrefix}.Text`;
        const questionValidation = card.querySelector('span[data-valmsg-for*=".Text"]');
        questionValidation.setAttribute('data-valmsg-for', `${namePrefix}.Text`);

        // Rename answers and correct radio
        const radios = card.querySelectorAll('input[type="radio"]');
        const answerInputs = card.querySelectorAll('input[type="text"][name*="Answers"]');
        const answerValidations = card.querySelectorAll('span[data-valmsg-for*="Answers"]');

        radios.forEach((radio, idx) => {
            radio.name = `${namePrefix}.Correct`;
            radio.value = idx;
        });

        answerInputs.forEach((input, idx) => {
            input.name = `${namePrefix}.Answers[${idx}].AnswerText`;
        });

        answerValidations.forEach((span, idx) => {
            span.setAttribute('data-valmsg-for', `${namePrefix}.Answers[${idx}].AnswerText`);
        });

        questionIndex++;
    });

    // Re-parse entire form after renaming
    $.validator.unobtrusive.parse("#question-container");
}

function markCorrect(radio) {
    const groupName = radio.name;
    const allOptions = document.querySelectorAll(`input[name="${groupName}"]`);
    allOptions.forEach(r => {
        const wrapper = r.closest('.form-check');
        wrapper.classList.remove('correct-selected');
    });
    radio.closest('.form-check').classList.add('correct-selected');
}
