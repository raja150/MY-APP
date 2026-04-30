import React from 'react'
import * as dateUtil from 'utils/date';
export default function DateDetails(props) {
    return (
        <div style={{ color: '#458bd6' }}>
            <span>{dateUtil.getDate(props.value)}</span>
            {props.value != props.row.original.toDate ? <div><span style={{ color: 'black'}}> to </span><br />
                <span>{dateUtil.getDate(props.row.original.toDate)}</span></div> : ''}
        </div>
    )

}