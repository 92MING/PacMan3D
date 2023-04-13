import { List, Avatar } from '@arco-design/web-react';
<<<<<<< HEAD
import { IconHeart, IconMessage, IconStar } from '@arco-design/web-react/icon';
import './Blog.css'
const names = ['Socrates', 'Balzac', 'Plato', 'Putin'];
const avatarSrc = [
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/a8c8cdb109cb051163646151a4a5083b.png~tplv-uwbnlip3yd-webp.webp',
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/e278888093bef8910e829486fb45dd69.png~tplv-uwbnlip3yd-webp.webp',
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/9eeb1800d9b78349b24682c3518ac4a3.png~tplv-uwbnlip3yd-webp.webp',
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/a8c8cdb109cb051163646151a4a5083b.png~tplv-uwbnlip3yd-webp.webp',
];
const imageSrc = [
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/29c1f9d7d17c503c5d7bf4e538cb7c4f.png~tplv-uwbnlip3yd-webp.webp',
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/04d7bc31dd67dcdf380bc3f6aa07599f.png~tplv-uwbnlip3yd-webp.webp',
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/1f61854a849a076318ed527c8fca1bbf.png~tplv-uwbnlip3yd-webp.webp',
  '//p1-arco.byteimg.com/tos-cn-i-uwbnlip3yd/29c1f9d7d17c503c5d7bf4e538cb7c4f.png~tplv-uwbnlip3yd-webp.webp',
];
const dataSource = new Array(15).fill(null).map((_, index) => {
  return {
    index: index,
    avatar: avatarSrc[index % avatarSrc.length],
    title: names[index % names.length],
    description: 'test test test test',
    imageSrc: imageSrc[index % imageSrc.length],
  };
});

const App = () => {
=======
import { IconHeart, IconUser, IconStar } from '@arco-design/web-react/icon';
import './Blog.css'
import { AuthContext } from "../component/AuthContext";
import React, { Component, useState, useEffect,useContext } from "react";
import axios from 'axios';
import {Message} from '@arco-design/web-react';
import { Modal} from '@arco-design/web-react';

const App = () => {
  const[blogList, setBlogList] = useState([]);
  const [visible2, setVisible2] = React.useState(false);
  const [like, setLike] = React.useState(false);
  const [blogtitle, setBlogtitle] = React.useState('');
  const [blogcontent, setBlogcontent] = React.useState('');

  useEffect(() => {
    axios.get('http://localhost:3000/api/blog')
      .then(res => {
        // Sort the blog list in descending order based on the creation date
        const sortedList = res.data.sort((a, b) => {
          return new Date(b.createdAt) - new Date(a.createdAt);
        });
        setBlogList(sortedList);
      })
      .catch(error => {
        console.error(error);
      });
  }, []);

  const names = [];
  const author=[];
  const description=[];
  const heart=[];
  const id=[];

  for (var i = 0; i < blogList.length; i++) {
    names.push(blogList[i].title);
    author.push(blogList[i].creatorId);
    description.push(blogList[i].content);
    heart.push(blogList[i].numberOfLikes);
    id.push(blogList[i]._id);
  }
  
  const dataSource = new Array(blogList.length).fill(null).map((_, index) => {
    return {
      index: index,
      title: names[index % names.length],
      author: author[index % author.length],
      description: description[index % description.length],
      heart: heart[index % heart.length],
      id: id[index % id.length],
    };
  });

  async function addlike (blog_id){
    const url='http://localhost:3000/api/blog/like';
    try{
      console.log(blog_id);
      const res = await axios.post(url, { blog_id, });
      console.log(res.data);
      if (res.data.isLiked ) {
        console.success('Success！');
      } else{
        Message.error('Failed！');
      }
    }
    catch (e) {
      Message.error('Server is down！');
      console.error(e);
    }
  }

  async function showdetail (detail){
    console.log(detail);
  }

>>>>>>> master
  return (
    <div className='demo'>
    <List
      className='list-demo-action-layout'
<<<<<<< HEAD
      wrapperStyle={{ maxWidth: 1314 }}
      bordered={false}
      pagination={{ pageSize: 3 }}
=======
      noDataElement={
        <div className="arco-list arco-list-default arco-list-no-border list-demo-action-layout"><div role="list" className="arco-list-content"><div className="arco-empty"><div className="arco-empty-wrapper"><div className="arco-empty-image"><svg fill="none" stroke="currentColor" strokeWidth="4" viewBox="0 0 48 48" aria-hidden="true" focusable="false" className="arco-icon arco-icon-empty"><path d="M24 5v6m7 1 4-4m-18 4-4-4m28.5 22H28s-1 3-4 3-4-3-4-3H6.5M40 41H8a2 2 0 0 1-2-2v-8.46a2 2 0 0 1 .272-1.007l6.15-10.54A2 2 0 0 1 14.148 18H33.85a2 2 0 0 1 1.728.992l6.149 10.541A2 2 0 0 1 42 30.541V39a2 2 0 0 1-2 2Z"></path></svg></div><div className="arco-empty-description">No Data</div></div></div></div></div>
      }
      wrapperStyle={{ maxWidth: 1314 }}
      bordered={false}
      pagination={{ pageSize: 4 }}
>>>>>>> master
      dataSource={dataSource}
      render={(item, index) => (
        <List.Item
          key={index}
          style={{ padding: '20px 0', borderBottom: '1px solid var(--color-fill-3)' }}
          actionLayout='vertical'
          actions={[
            <span key={1}>
<<<<<<< HEAD
              <IconHeart />
              {83}
            </span>,
          ]}
          extra={
            <div className='image-area'>
              <img alt='arcodesign' src={item.imageSrc} />
            </div>
          }
        >
          <List.Item.Meta
            avatar={
              <Avatar shape='square'>
                <img alt='avatar' src={`${item.avatar}`} />
              </Avatar>
            }
            title={item.title}
            description={item.description}
          />
=======
              <IconHeart onClick={()=>{addlike(item.id); }}/>
              {item.heart}
            </span>,
          ]}
        >
          <List.Item.Meta
            avatar={
              item.author
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
            showdetail(item);
            setVisible2(false);
          }}
        >
          <div className='test'>{blogcontent}</div>
        </Modal>
>>>>>>> master
        </List.Item>
      )}
    />
    </div>
  );
};

export default App;