import React, { useState, useEffect } from 'react'

export default function Timer(props) {
    const { timeLeft, setTimeOver, setUnSavedValues, questions } = props
    const [counter, setCounter] = useState(timeLeft < 0 ? 0 : timeLeft);
    useEffect(() => {
        if (counter > 0) {
            setTimeout(() => setCounter(counter - 1), 1000);
        }
        else {
            setTimeOver(true);
        }
        window.onbeforeunload = (e) => {
            if (!localStorage.getItem('finishExam')) {
                setUnSavedValues(questions);
            }
            localStorage.removeItem('finishExam');
        };
    }, [counter]);

    return `${Math.floor(counter / 60)}`.padStart(2, 0) + ':' + `${counter % 60}`.padStart(2, 0)
}