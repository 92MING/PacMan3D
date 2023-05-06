# Backend Functions

## User

1. signUp(username, email, password)
<br>Input: username (string), email (string), password (string)
<br>Output: isCreated 

2. login(email, password)
<br>Input: email (string), password (string)
<br>Output: isLogin

3. reset(username, oldPassword, newPassword)
<br>Input: username (string), oldPassword (string), newPassword (string)
<br>Output: isReset

4. retrieveUser(username)
<br>Input: username (string)

5. deleteUser(username)
<br>Input: username (string)
<br>Output: isDeleted

## Blog
1. addBlog(title, content, creatorId)
<br>Input: title (string), content (string), creatorId (string)
<br>Output: isAdded

2. getBlogByUser(creatorId)
<br>Input: creatorId (string)
<br>Output: [{title, content, creatorId, createdAt}]

3. deleteBlog(blogId)
<br>Input: blogId (string)
<br>Output: isDeleted

4. likeBlog(blogId)
<br>Input: blogId (string)
<br>Output: isLiked

## Map
1. saveMap(id, creatorID, name, mapSize, mapCells)
<br>Input: id (string), creatorID (string), name (string), mapSize (string), mapCells (array of objects with properties: type (number), objName (string), direction (number))
<br>Output: isSaved

2. getMapById(id)
<br>Input: id (string)
<br>Output: {id, creatorID, name, mapSize, mapCells, createdAt}

3. getMapsByCreatorID(creatorID)
<br>Input: creatorID (string)
<br>Output: [{id, creatorID, name, mapSize, mapCells, createdAt}]

4. deleteMap(id)
<br>Input: id (string)
<br>Output: isDeleted