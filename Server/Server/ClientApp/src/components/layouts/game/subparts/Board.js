import React, { Component } from 'react';

import './Board.css';

export class Board extends Component {

    constructor(props) {
        super(props);
        this.state = {
            size: 100,
            currentX: -window.innerWidth / 2,
            currentY: -window.innerHeight / 2,
            isDragging: false,
            ghost: false,
            ghostR: 0
        };

        this.lastX = 0;
        this.lastY = 0;
        this.moved = 0;
    }

    mouseDown(e) {
        this.lastX = this.state.currentX + e.clientX;
        this.lastY = this.state.currentY + e.clientY;
        this.moved = 0;
        this.setState({ isDragging: true });
    }

    mouseMove(e) {
        if (!this.state.isDragging) {
            var x = Math.floor((this.state.currentX + e.clientX) / this.state.size);
            var y = Math.floor((this.state.currentY + e.clientY) / this.state.size);
            if (this.state.ghost == false || this.state.ghost.x != x || this.state.ghost.y != y) {
                var pieces = this.props.pieces;
                var found = false;
                for (let i in pieces) {
                    let p = pieces[i];
                    if (p.x == x && p.y == y) {
                        found = true;
                        break;
                    }
                }
                if (found) {
                    if (this.state.ghost !== false) this.setState({ ghost: false });
                    return;
                }
                this.setState({ ghost: { x: x, y: y } });
            }
            return;
        }
        this.moved++;
        this.setState({
            currentX: this.lastX - e.clientX,
            currentY: this.lastY - e.clientY
        })
    }

    mouseUp(e) {
        this.setState({ isDragging: false });
        if (this.moved < 10) {
            // click
        }
    }

    mouseLeave(e) {
        this.setState({ ghost: false });
    }

    scroll(e) {
        var newSize, nX, nY, x, y;
        var plus = e.deltaY < 0
        newSize = plus ? this.state.size * 2 : this.state.size / 2;
        x = this.state.currentX + e.clientX;
        y = this.state.currentY + e.clientY;
        nX = this.state.currentX + (plus ? x : -x / 2);
        nY = this.state.currentY + (plus ? y : -y / 2);
        if (newSize < 20 || newSize > 200) return;
        this.setState({ size: newSize, currentX: nX, currentY: nY });
    }

    click(e) {
        if (e.nativeEvent.button === 2) {
            this.setState({ ghostR: (this.state.ghostR + 1) % 4 });
            e.preventDefault();
        }
    }

    render() {
        return (
            <div className="Board"
                onMouseDown={this.mouseDown.bind(this)}
                onMouseMove={this.mouseMove.bind(this)}
                onMouseUp={this.mouseUp.bind(this)}
                onWheel={this.scroll.bind(this)}
                onMouseLeave={this.mouseLeave.bind(this)}
                onClick={this.click.bind(this)}
                onContextMenu={this.click.bind(this)}
                style={{
                    backgroundPositionX: -this.state.currentX,
                    backgroundPositionY: -this.state.currentY,
                    backgroundSize: this.state.size * 3,
                }}>
                {this.props.pieces.map((p, i) => (
                    <div key={i} className="piece"
                        style={{
                            width: this.state.size,
                            height: this.state.size,
                            left: -this.state.currentX + p.x * this.state.size,
                            top: -this.state.currentY + p.y * this.state.size,
                            backgroundImage: 'url(/graphics/bitmap?name=' + p.bitmap + ')',
                            backgroundSize: this.state.size,
                            transform: 'rotate(' + p.r * 90 + 'deg)'
                        }}>
                    </div>
                ))}
                {
                    this.state.ghost ? (<div className="piece ghost" style={{
                        width: this.state.size,
                        height: this.state.size,
                        left: -this.state.currentX + this.state.ghost.x * this.state.size,
                        top: -this.state.currentY + this.state.ghost.y * this.state.size,
                        backgroundImage: 'url(' + this.props.currentImg + ')',
                        backgroundSize: this.state.size,
                        transform: 'rotate(' + this.state.ghostR * 90 + 'deg)'
                    }} />) : ''
                }
            </div>
        );
    }
};