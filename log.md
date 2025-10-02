2025.10.02
- 搭建了Map1，原本想使用.tmx的map文件利用SuperTiled2Unity插件直接导入（耗费了非常久的时间），但是经过多次尝试都失败了（无法构建单向平台机制，只能是个普通地图），最后只能手动搭建地图

- 手动搭建使用Unity的Tilemap功能，一个个地图方块设置Prefab，然后放进Tilemap palette中的时候出现bug：有时候无法直接拖拽Prefab到Tilemap palette中，会报错。最终只能多创建几个Tilemap palette。

- 创建了Player的动画状态机，包含Idle、Run、Jump、Down、Shot状态，实现了部分动画切换

- 发现地图的一些方块还没设置好两侧的单向平台碰撞，导致角色可以从侧面穿过去，明天继续完善