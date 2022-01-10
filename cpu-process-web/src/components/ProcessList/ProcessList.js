import React, { useState, useEffect } from 'react';
import { List, AutoSizer, CellMeasurer, CellMeasurerCache } from 'react-virtualized';
import Process from '../Process/Process';
import './processList.css';

const cache = new CellMeasurerCache({
    fixedWidth: true,
    defaultHeight: 100
});



const ProcessList = (props) => {
    const sortingMode = props.sortingMode;

    let { totalRamUsage, totalCpuUsage, processes } = props.data;
    totalRamUsage = totalRamUsage.toFixed(1) + "%";
    totalCpuUsage = totalCpuUsage.toFixed(1) + "%";

    /*let groups = [];
    processes.forEach(item => {
        let index = groups.findIndex(group => group.id === item.group);
        let group = groups[index] || {};
        if (group && Object.keys(group).length === 0) {
            group.id = item.group;
            group.name = item.groupDescription;
            group.ramUsage = item.ramUsage;
            group.cpuUsage = item.cpuUsage;
            group.items = [item];
            groups.push(group);
        }
        else {
            group.ramUsage += item.ramUsage;
            group.cpuUsage += item.cpuUsage;
            group.items.push(item);
            groups[index] = group;
        }
    });
    groups = sortProcess(groups, sortingMode);*/

    const sortProcess = (procs, sortMode) => {
        switch (sortMode) {
            case "RamAsc":
                return procs.sort((a, b) => a.ramUsage > b.ramUsage ? 1 : -1)
            case "RamDesc":
                return procs.sort((a, b) => a.ramUsage < b.ramUsage ? 1 : -1)
            case "CpuAsc":
                return procs.sort((a, b) => a.cpuUsage > b.cpuUsage ? 1 : -1)
            case "CpuDesc":
                return procs.sort((a, b) => a.cpuUsage < b.cpuUsage ? 1 : -1)
            default:
                break;
        }
    }

    processes = sortProcess(processes, sortingMode);

    const renderRow = ({ index, key, style, parent }) => {
        return (
            <CellMeasurer
                key={key}
                cache={cache}
                parent={parent}
                columnIndex={0}
                rowIndex={index}>
                <div style={style} className="row">
                    <div className="content">
                        <Process process={processes[index]} sortingMode={sortingMode} />
                    </div>
                </div>
            </CellMeasurer>
        );
    }

    return (

        <div className='processList'>
            <div className='listHeader'>
                <div className='id'>Id</div>
                <div className='name'>Name</div>
                <div
                    onClick={() => props.toggleSortingMode("Ram")}
                    className={sortingMode.includes('Ram') ? 'ram sortable sorted' : "ram sortable"}>
                    Ram {totalRamUsage}<span>{sortingMode == "RamDesc" ? "▼" : "▲"}</span>
                </div>
                <div
                    onClick={() => props.toggleSortingMode("Cpu")}
                    className={sortingMode.includes('Cpu') ? 'cpu sortable sorted' : "cpu sortable"}>
                    Cpu {totalCpuUsage}<span>{sortingMode == "CpuDesc" ? "▼" : "▲"}</span>
                </div>
            </div>
            <div className='listItems'>
                <AutoSizer>
                    {({ width, height }) => {
                        return <List
                            width={width}
                            height={height}
                            deferredMeasurementCache={cache}
                            rowHeight={cache.rowHeight}
                            rowRenderer={renderRow}
                            rowCount={processes.length}
                            overscanRowCount={5} />
                    }}
                </AutoSizer>
            </div>
        </div>
    );
};


export default ProcessList;