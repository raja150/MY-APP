import React from 'react';
import APIService from '../../services/apiservice';

export default function CascadeDropdown(){
    const q = value ? `LmsReport/FunctionalArea?lobId=${value}` : 'LmsReport/FunctionalArea';
    await APIService.getAsync(q).then(res => {
        ddValues['functionalAreaId'].data = res.data;
        ddValues['designationId'].data = [];
        vv.functionalAreaId = '';
        vv.designationId = '';

    }).catch(err => {
        notifyError(err.message)
    })
}