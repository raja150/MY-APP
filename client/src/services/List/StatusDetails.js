import React from 'react';
export default function StatusDetails(props) {
    return (
        <div >
            {(props.value != 'Applied' && props.value != 'Cancelled')? <div><span style={{ color: 'black' }}>{props.value} by </span><br />
                <span>{props.row.original.approvedBy}</span></div> : <span>{props.value}</span>}
        </div>
    )
}