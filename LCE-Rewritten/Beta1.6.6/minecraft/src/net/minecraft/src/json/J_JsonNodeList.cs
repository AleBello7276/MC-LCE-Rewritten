namespace net.minecraft.src.json
{
	internal sealed class J_JsonNodeList : List<J_JsonNode> {
		 private readonly IEnumerable<J_JsonNode> _items;

		public J_JsonNodeList(IEnumerable<J_JsonNode> items) {
			_items = items;
            foreach (var item in _items)
            {
                Add(item);
            }

		}
	}
}



