import { List, Avatar } from '@arco-design/web-react';
import { IconHeart, IconDelete } from '@arco-design/web-react/icon';
import React, { Component, useState, useEffect,useContext } from "react";
import './Blog.css'
import { AuthContext } from "../component/AuthContext";
import axios from 'axios';
import {Message, Modal} from '@arco-design/web-react';

const App = () => {
  const { user } = useContext(AuthContext);
  const [visible2, setVisible2] = React.useState(false);
  const[blogList, setBlogList] = useState([]);
  const [blogtitle, setBlogtitle] = React.useState('');
  const [blogcontent, setBlogcontent] = React.useState('');

  useEffect(() => {
      const url='http://localhost:3000/api/blog';
      axios.get(url+"/"+user).then(res=>{
      console.log(res.data);
      setBlogList(res.data);
    })},[user])

    const names = [];
    const author=[];
    const description=[];
    const heart=[];
    const id=[];
    const date=[];
  
    for (var i = 0; i < blogList.length; i++) {
      names.push(blogList[i].title);
      author.push(blogList[i].creatorID);
      description.push(blogList[i].content);
      heart.push(blogList[i].numberOfLikes);
      id.push(blogList[i]._id);
      date.push(blogList[i].createdAt);
    }

  const dataSource = new Array(blogList.length).fill(null).map((_, index) => {
    return {
      index: index,
      title: names[index % names.length],
      author: author[index % author.length],
      description: description[index % description.length],
      heart: heart[index % heart.length],
      id: id[index % id.length],
      date: date[index % date.length],
    };
  });

  function getDateTime(time) {
    var date = new Date(time);
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hh = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
    var mm = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
    var ss = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
    return date.getFullYear() + "-" + month + "-" + currentDate+" "+hh + ":" + mm+":"+ss;
}

  return (
    <div className='demo'>
    <List
      className='list-demo-action-layout'
      noDataElement={
        <div className="arco-list arco-list-default arco-list-no-border list-demo-action-layout"><div role="list" className="arco-list-content"><div className="arco-empty"><div className="arco-empty-wrapper"><div className="arco-empty-image"><svg fill="none" stroke="currentColor" strokeWidth="4" viewBox="0 0 48 48" aria-hidden="true" focusable="false" className="arco-icon arco-icon-empty"><path d="M24 5v6m7 1 4-4m-18 4-4-4m28.5 22H28s-1 3-4 3-4-3-4-3H6.5M40 41H8a2 2 0 0 1-2-2v-8.46a2 2 0 0 1 .272-1.007l6.15-10.54A2 2 0 0 1 14.148 18H33.85a2 2 0 0 1 1.728.992l6.149 10.541A2 2 0 0 1 42 30.541V39a2 2 0 0 1-2 2Z"></path></svg></div><div className="arco-empty-description">No Data</div></div></div></div></div>
      }
      wrapperStyle={{ maxWidth: 1314 }}
      bordered={false}
      pagination={{ pageSize: 4 }}
      dataSource={dataSource}
      render={(item, index) => (
        <List.Item
          key={index}
          style={{ padding: '20px 0', borderBottom: '1px solid var(--color-fill-3)' }}
          actionLayout='vertical'
          actions={[
            <span key={2} onClick={async (e) => {
                const url="http://localhost:3000/api/blog"
                try{
                  const res = await axios.delete(url+"/"+item.id);
                  if (res.data.isDeleted ) {
                    Message.success('Successfully deleted！');
                  } else{
                    Message.error('Failed！');
                  }
                }
                catch (e) {
                  Message.error('Server is down！');
                  console.error(e);
                }
              }}>
                <IconDelete />
                Delete
            </span>
          ]}
        >
          <List.Item.Meta
            avatar={
                getDateTime(item.date)
            }
            title={item.title}
            description={item.description}
            onClick={() => {
              setBlogtitle(item.title);
              setBlogcontent(item.description);
              setVisible2(true);
            }}
          />
          <Modal
        className='modal'
        title={blogtitle}
        visible={visible2}
        footer={null}
        onCancel={() => {
          setVisible2(false);
        }}
      >
        <div className='test'>{blogcontent}</div>
      </Modal>
        </List.Item>
      )}
    />
    </div>
  );
};

export default App;