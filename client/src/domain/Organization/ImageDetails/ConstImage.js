import React from 'react';
import avatar from 'assets/utils/images/avatars/1.jpg';

export function EmployeeImageCell(id) {
    return <div>
        <img width="40" className="rounded-circle"
            src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/avatars/${id}.jpg`} alt="avatars"
            onError={(e) => (e.target.onerror = null, e.target.src = avatar)} />
    </div>
}
export const EmployeeImageProps =
{
    name: 'image', label: '', accessor:'image',type: 'custom', disableSorting: true, width: "50"
}