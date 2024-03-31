namespace DiscUtils.Hfs
{
internal delegate int BTreeVisitor<Key>(Key key, byte[] data)
where Key : BTreeKey;
}