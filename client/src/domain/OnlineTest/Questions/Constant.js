export const QUESTIONS_TYPES = [
    { id: 1, text: 'Single Correct' },
    { id: 2, text: 'Multi Correct' },
    { id: 3, text: 'Numerical Answer' },
    { id: 4, text: 'True or False' },
    { id: 5, text: 'Text-Box Subjective' }
]

// for Question List
export const QUES_TYPES = [
    { id: 1, text: 'Single' },
    { id: 2, text: 'Multiple' },
    { id: 3, text: 'Numeric' },
    { id: 4, text: 'T or F' },
    { id: 5, text: 'Text' }
]

export const SINGLE_CORRECT = [
    { value: 'A', text: 'A' }, { value: 'B', text: 'B' },
]

export const MULTI_CORRECT = [
    { value: 'A', text: 'A' }, { value: 'B', text: 'B' },
    { value: 'C', text: 'C' }, { value: 'D', text: 'D' },
]

export const TRUE_FALSE = [{ value: 'true', text: 'True' }, { value: 'false', text: 'False' }]

export const NUMBER_TO_ALPHABET = (num) => {
    switch (num) {
        case 0:
            return 'A'
        case 1:
            return 'B'
        case 2:
            return 'C'
        case 3:
            return 'D'
        case 4:
            return 'E'
        case 5:
            return 'F'
    }
}


export const REMOVE_LAST_CHAR = (str) => {
    return str.substring(0, str.length - 1);
}

export const LINE_ITEM = {
    text: '', type: '', key: '',
    choices: [
        { sNo: '', text: '', key: '' },
        { sNo: '', text: '', key: '' },
    ]
}

export const CHOICE = [
    { sNo: '', text: '', key: '' },
    { sNo: '', text: '', key: '' }
]
