import Table from 'domain/PayRuns/PayRun/table';
import React, { useEffect, useState } from 'react';
import { Button, Card, Col, Row } from 'reactstrap';
import TeamService from 'services/Org/Team';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { notifySaved } from 'components/alert/Toast';
import * as formUtil from 'utils/form';
import queryString from 'query-string';


export default function TeamWeekOffList(props) {
    const TeamColumns = [
        {
            Header: 'Team Name',
            accessor: 'name'
        },
        {
            Header: 'Delete',
            accessor: 'delete', Cell: ({ value,row }) => <FontAwesomeIcon icon={faTrash} style={{ color: 'red' }} onClick={() => handleDelete(row.original)} />
        }

    ]
    const [state, setState] = useState({ isLoading: true, weekOffs: [], pages: -1, title: '', pageSize: 10, hasNext: false, hasPrevious: false })
    const [tableLoading, setTableLoading] = useState(true)

    const fetchData = async () => {
        let pages = '', hasNext = false, hasPrevious = false, pageIndex = 0;

        const qString = queryString.stringify({
            page: 0,
            size: 10,
            refId: props.refId
        });
        await TeamService.getTeamWeekOffList(qString).then((res) => {
            pages = res.data.pages;
            hasNext = res.data.hasNext;
            hasPrevious = res.data.hasPrevious;
            pageIndex = res.data.index
            setState({ ...state, isLoading: false, weekOffs: res.data.items, pages: pages, hasNext: hasNext, hasPrevious: hasPrevious, pageIndex: pageIndex })
        })
    }
    useEffect(() => {
        fetchData();
    }, [])
   
    const handleSearch = async (page, pageSize, sortBy, isDescend) => {
        const qString = queryString.stringify({
            page,
            size: pageSize || state.pageSize,
            refId: props.refId,
            name: state.empNo,
            sortBy: sortBy || '', isDescend: isDescend || 'false'
        });
        setTableLoading(true);

        await TeamService.getTeamWeekOffList(qString).then((res) => {
            setState({
                ...state, pages: res.data.pages, weekOffs: res.data.items, pageSize: pageSize || state.pageSize,
                hasNext: res.data.hasNext, hasPrevious: res.data.hasPrevious, pageIndex: res.data.index
            });
        });
        setTableLoading(false);
    }

    const handleDelete = async (n) => {
        await TeamService.DeleteWeekOffSetup(n).then((res) => {
            notifySaved('Deleted Successfully');
            fetchData();
        }).catch((err) => {
            formUtil.displayErrors(err)
        })
    }
    return <>
        <Card className='mb-4'>
            <Row>
                <Col md='10'></Col>
                <Col md='2'>
                    <Button className="mb-2 mr-2 btn-icon float-right btn-xs btn-primary1" color="primary" type="button"
                        onClick={() => props.handleAddTeam(null)}
                    >
                        <i className="pe-7s-plus btn-icon-wrapper font-weight-bold" />
                        ADD
                    </Button>
                </Col>
            </Row>
            <Table columns={TeamColumns} data={state.weekOffs} pages={state.pages} hasPrevious={state.hasPrevious} hasNext={state.hasNext} pageIndex={state.pageIndex} showPaginate={true}  handleSearch={handleSearch}/>
        </Card>
    </>
}