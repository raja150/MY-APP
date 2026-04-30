import Loading from 'components/Loader';
import * as _ from 'lodash';
import React, { Fragment, useEffect, useState } from 'react';
import { CardTitle } from 'reactstrap';
import PayMonth from 'services/PayRoll/PayMonth';
import Table from '../table';
import { TAX_COLUMNS, TAX_DEDUCTIONS } from './ConstValues';


function Index(props) {

    const [state, setState] = useState({ isLoading: true, taxes: [], deductions: [] })

    useEffect(() => {
        const fetchData = async () => {
            let taxes = [], deductions = []
            await PayMonth.Taxes(props.rid).then((res) => {
                if (!_.isEmpty(res.data)) {
                    if (res.data.taxes.length > 0) {
                        taxes = res.data.taxes
                    }
                    if (res.data.deductions.length > 0) {
                        deductions = res.data.deductions
                    }
                }
            })
            setState({ ...state, isLoading: false, taxes: taxes, deductions: deductions })
        }
        fetchData()
    }, [])
    return (
        state.isLoading ? <Loading /> :
            <Fragment>
                <div>
                    <CardTitle>Tax Details</CardTitle>
                    <Table columns={TAX_COLUMNS} data={state.taxes} showPaginate={false} />
                </div>
                <div>
                    <CardTitle>Pre-Tax Deductions</CardTitle>
                    <Table columns={TAX_DEDUCTIONS} data={state.deductions} showPaginate={false} />
                </div>
            </Fragment>
    )
}

export default Index
