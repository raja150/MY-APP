import React from 'react';
export default function TableHeader(props) {
    return (
        <thead>
            <tr>
                <th key='sno'>S.No.</th>
                {
                    props.columns.length > 0
                    && props.columns.sort((a, b) => a.orderBy - b.orderBy).map((column, i) => {
                        return <th key={i}>{column.label}</th>
                    })
                }
            </tr>
        </thead>
    )
}